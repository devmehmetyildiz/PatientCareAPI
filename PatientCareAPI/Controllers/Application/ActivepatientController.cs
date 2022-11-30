using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PatientCareAPI.DataAccess;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Models.Application;
using PatientCareAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using PatientCareAPI.Models.Settings;

namespace PatientCareAPI.Controllers.Application
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ActivepatientController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<ActivepatientController> _logger;
        private readonly ApplicationDBContext _context;
        Utilities Utilities;
        UnitOfWork unitOfWork;
        public ActivepatientController(IConfiguration configuration, ILogger<ActivepatientController> logger, ApplicationDBContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
            Utilities = new Utilities(context);
            unitOfWork = new UnitOfWork(context);
        }

        [Route("GetAll")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Screen)]
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ActivepatientModel> Data = new List<ActivepatientModel>();
            if (Utilities.CheckAuth(UserAuthory.Patients_ManageAll, this.User.Identity))
            {
                //  Data = unitOfWork.ActivepatientRepository.GetAll().Where(u => u.IsActive).ToList();
                Data = unitOfWork.ActivepatientRepository.GetRecords<ActivepatientModel>(u => u.IsActive).ToList();
                foreach (var item in Data)
                {
                    item.Patient = unitOfWork.PatientRepository.GetPatientByGuid(item.PatientID);
                    //TODO process eklenecek
                }
            }
            else
            {
                Data = unitOfWork.ActivepatientRepository.GetAll().Where(u => u.IsActive && u.CreatedUser == this.User.Identity.Name).ToList();
                foreach (var item in Data)
                {
                    item.Patient = unitOfWork.PatientRepository.GetPatientByGuid(item.PatientID);
                }
            }
            if (Data.Count == 0)
                return NotFound();
            return Ok(Data);
        }

        [Route("GetSelectedActivepatient")]
        [AuthorizeMultiplePolicy((UserAuthory.Patients_Screen + "," + UserAuthory.Patients_Update))]
        [HttpGet]
        public IActionResult GetSelectedCase(string Guid)
        {
            ActivepatientModel Data = unitOfWork.ActivepatientRepository.FindByGuid(Guid);
            Data.Patient = unitOfWork.PatientRepository.GetPatientByGuid(Data.PatientID);
            if (!Utilities.CheckAuth(UserAuthory.Patients_ManageAll, this.User.Identity))
            {
                if (Data.CreatedUser != this.User.Identity.Name)
                {
                    return StatusCode(403);
                }
            }
            if (Data == null)
            {
                return NotFound();
            }
            return Ok(Data);
        }

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Add)]
        [HttpPost]
        public IActionResult Add(ActivepatientModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            string patientguid = Guid.NewGuid().ToString();
            if (model.Patient != null)
            {
                model.Patient.ConcurrencyStamp = patientguid;
                model.Patient.CreatedUser = username;
                model.Patient.IsActive = true;
                model.Patient.CreateTime = DateTime.Now;
                model.PatientID = patientguid;
                unitOfWork.PatientRepository.Add(model.Patient);
            }
            string guid = Guid.NewGuid().ToString();
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = guid;
            unitOfWork.ActivepatientRepository.Add(model);
            unitOfWork.Complate();
            return Ok(guid);
        }

        [Route("GetUserImage")]
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetUserImage(string Guid)
        {
            FileModel Data = unitOfWork.FileRepository.GetFilebyGuid(unitOfWork.ActivepatientRepository.FindByGuid(Guid).ImageID);
            if (Data != null)
                return File(Utilities.GetFile(Data), Data.Filetype);
            else
                return NotFound();
        }

        [Route("AddImage")]
        [AuthorizeMultiplePolicy(UserAuthory.File_Add)]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [HttpPost]
        public IActionResult AddImage([FromForm] FileModel model)
        {
            var username = (this.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrWhiteSpace(model.Filefolder))
            {
                model.Filefolder = Guid.NewGuid().ToString();
            }
            string imageguid = Guid.NewGuid().ToString();
            ActivepatientModel patientmodel = unitOfWork.ActivepatientRepository.FindByGuid(model.ConcurrencyStamp);
            patientmodel.ImageID = imageguid;
            PatientModel patient = unitOfWork.PatientRepository.GetPatientByGuid(patientmodel.PatientID);
            unitOfWork.ActivepatientRepository.update(unitOfWork.ActivepatientRepository.FindByGuid(model.ConcurrencyStamp), patientmodel);
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = imageguid;
            model.Name = patient.Firstname+patient.Lastname;
            model.Filetype = model.File.ContentType;
            model.Filename = model.File.FileName;
            if (Utilities.UploadFile(model))
            {
                unitOfWork.FileRepository.Add(model);
                unitOfWork.Complate();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }


        [Route("Update")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Update)]
        [HttpPost]
        public IActionResult Update(ActivepatientModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            model.PatientID = model.Patient.ConcurrencyStamp;
            if (!Utilities.CheckAuth(UserAuthory.Patients_ManageAll, this.User.Identity))
            {
                if (model.CreatedUser == this.User.Identity.Name)
                {
                    return StatusCode(403);
                }
            }
            unitOfWork.ActivepatientRepository.update(unitOfWork.ActivepatientRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Delete")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Delete)]
        [HttpDelete]
        public IActionResult Delete(ActivepatientModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            if (!Utilities.CheckAuth(UserAuthory.Patients_ManageAll, this.User.Identity))
            {
                if (model.CreatedUser == this.User.Identity.Name)
                {
                    return StatusCode(403);
                }
            }
            unitOfWork.ActivepatientRepository.update(unitOfWork.ActivepatientRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("DeleteFromDB")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(ActivepatientModel model)
        {
            unitOfWork.ActivepatientRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
