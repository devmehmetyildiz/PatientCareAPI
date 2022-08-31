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
        [Authorize(Roles = UserAuthory.Patients_Screen)]
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ActivepatientModel> Data = new List<ActivepatientModel>();
            if (Utilities.CheckAuth(UserAuthory.Patients_ManageAll, this.User.Identity))
            {
                Data = unitOfWork.ActivepatientRepository.GetAll().Where(u => u.IsActive).ToList();
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

        [Authorize(Roles = (UserAuthory.Patients_Screen + "," + UserAuthory.Patients_Update))]
        [Route("GetSelectedActivepatient")]
        [HttpGet]
        public IActionResult GetSelectedCase(int ID)
        {
            ActivepatientModel Data = unitOfWork.ActivepatientRepository.Getbyid(ID);
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
        [Authorize(Roles = UserAuthory.Patients_Add)]
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

            if (model.Applicant != null)
            {
                model.Applicant.Activepatientid = guid;
                model.Applicant.CreatedUser = username;
                model.Applicant.IsActive = true;
                model.Applicant.CreateTime = DateTime.Now;
                model.Applicant.ConcurrencyStamp = Guid.NewGuid().ToString();
                unitOfWork.PatientapplicantRepository.Add(model.Applicant);
            }
            if (model.Bodycontrolform != null)
            {
                model.Bodycontrolform.Activepatientid = guid;
                model.Bodycontrolform.CreatedUser = username;
                model.Bodycontrolform.IsActive = true;
                model.Bodycontrolform.CreateTime = DateTime.Now;
                model.Bodycontrolform.ConcurrencyStamp = Guid.NewGuid().ToString();
                unitOfWork.PatientbodycontrolformRepository.Add(model.Bodycontrolform);
            }
            if (model.Disabilitypermitform != null)
            {
                model.Disabilitypermitform.Activepatientid = guid;
                model.Disabilitypermitform.CreatedUser = username;
                model.Disabilitypermitform.IsActive = true;
                model.Disabilitypermitform.CreateTime = DateTime.Now;
                model.Disabilitypermitform.ConcurrencyStamp = Guid.NewGuid().ToString();
                unitOfWork.PatientdisabilitypermitformRepository.Add(model.Disabilitypermitform);
            }
            if (model.Disabledhealthboardreport != null)
            {
                model.Disabledhealthboardreport.Activepatientid = guid;
                model.Disabledhealthboardreport.CreatedUser = username;
                model.Disabledhealthboardreport.IsActive = true;
                model.Disabledhealthboardreport.CreateTime = DateTime.Now;
                model.Disabledhealthboardreport.ConcurrencyStamp = Guid.NewGuid().ToString();
                unitOfWork.PatientdisabledhealthboardreportRepository.Add(model.Disabledhealthboardreport);

            }
            if (model.Firstadmissionform != null)
            {
                model.Firstadmissionform.ActivepatientID = guid;
                model.Firstadmissionform.CreatedUser = username;
                model.Firstadmissionform.IsActive = true;
                model.Firstadmissionform.CreateTime = DateTime.Now;
                model.Firstadmissionform.ConcurrencyStamp = Guid.NewGuid().ToString();
                unitOfWork.PatientfirstadmissionformRepository.Add(model.Firstadmissionform);
            }
            if (model.Firstapproachreport != null)
            {
                model.Firstapproachreport.ActivepatientID = guid;
                model.Firstapproachreport.CreatedUser = username;
                model.Firstapproachreport.IsActive = true;
                model.Firstapproachreport.CreateTime = DateTime.Now;
                model.Firstapproachreport.ConcurrencyStamp = Guid.NewGuid().ToString();
                unitOfWork.PatientfirstapproachreportRepository.Add(model.Firstapproachreport);
            }
            if (model.Ownershiprecieve != null)
            {
                model.Ownershiprecieve.Activepatientid = guid;
                model.Ownershiprecieve.CreatedUser = username;
                model.Ownershiprecieve.IsActive = true;
                model.Ownershiprecieve.CreateTime = DateTime.Now;
                model.Ownershiprecieve.ConcurrencyStamp = Guid.NewGuid().ToString();
                unitOfWork.PatientownershiprecieveRepository.Add(model.Ownershiprecieve);
            }
            if (model.Recieveform != null)
            {
                model.Recieveform.Activepatientid = guid;
                model.Recieveform.CreatedUser = username;
                model.Recieveform.IsActive = true;
                model.Recieveform.CreateTime = DateTime.Now;
                model.Recieveform.ConcurrencyStamp = Guid.NewGuid().ToString();
                unitOfWork.PatientrecieveformRepository.Add(model.Recieveform);
            }
            if (model.Submittingform != null)
            {
                model.Submittingform.Activepatientid = guid;
                model.Submittingform.CreatedUser = username;
                model.Submittingform.IsActive = true;
                model.Submittingform.CreateTime = DateTime.Now;
                model.Submittingform.ConcurrencyStamp = Guid.NewGuid().ToString();
                unitOfWork.PatientsubmittingformRepository.Add(model.Submittingform);
            }
            unitOfWork.ActivepatientRepository.Add(model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Update")]
        [Authorize(Roles = UserAuthory.Patients_Update)]
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
        [Authorize(Roles = UserAuthory.Patients_Delete)]
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
        [Authorize(Roles = UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(ActivepatientModel model)
        {
            unitOfWork.ActivepatientRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
