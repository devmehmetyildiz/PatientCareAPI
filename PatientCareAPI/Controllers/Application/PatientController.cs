using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PatientCareAPI.DataAccess;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Models.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using PatientCareAPI.Utils;

namespace PatientCareAPI.Controllers.Application
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<PatientController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;
        Utilities Utilities;
        public PatientController(IConfiguration configuration, ILogger<PatientController> logger, ApplicationDBContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
            Utilities = new Utilities(context);
            unitOfWork = new UnitOfWork(context);
        }

        [HttpGet]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Screen)]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            List<PatientModel> Data = new List<PatientModel>();
            if (Utilities.CheckAuth(UserAuthory.Patients_ManageAll, this.User.Identity))
            {
                Data = unitOfWork.PatientRepository.GetAll().Where(u => u.IsActive).ToList();
                foreach (var item in Data)
                {
                    item.Patienttype = unitOfWork.PatienttypeRepository.GetPatienttypeByGuid(item.Patienttypeid);
                }
            }
            else
            {
                Data = unitOfWork.PatientRepository.GetAll().Where(u => u.IsActive && u.CreatedUser == this.User.Identity.Name).ToList();
                foreach (var item in Data)
                {
                    item.Patienttype = unitOfWork.PatienttypeRepository.GetPatienttypeByGuid(item.Patienttypeid);
                }
            }
            if (Data.Count == 0)
            {
                return NotFound();
            }
            return Ok(Data);
        }

        [Route("GetSelectedPatient")]
        [AuthorizeMultiplePolicy((UserAuthory.Patients_Screen + "," + UserAuthory.Patients_Update))]
        [HttpGet]
        public IActionResult GetSelectedPatient(int ID)
        {
            PatientModel Data = unitOfWork.PatientRepository.Getbyid(ID);
            Data.Patienttype = unitOfWork.PatienttypeRepository.GetPatienttypeByGuid(Data.Patienttypeid);
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
        public IActionResult Add(PatientModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            model.Patienttypeid = model.Patienttype.ConcurrencyStamp;
            unitOfWork.PatientRepository.Add(model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Update)]
        [HttpPost]
        public IActionResult Update(PatientModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            if (!Utilities.CheckAuth(UserAuthory.Patients_ManageAll, this.User.Identity))
            {
                if (model.CreatedUser == this.User.Identity.Name)
                {
                    return StatusCode(403);
                }
            }
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            model.Patienttypeid = model.Patienttype.ConcurrencyStamp;
            unitOfWork.PatientRepository.update(unitOfWork.PatientRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Delete")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Delete)]
        [HttpDelete]
        public IActionResult Delete(PatientModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            if (!Utilities.CheckAuth(UserAuthory.Patients_ManageAll, this.User.Identity))
            {
                if (model.CreatedUser == this.User.Identity.Name)
                {
                    return StatusCode(403);
                }
            }
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.PatientRepository.update(unitOfWork.PatientRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("DeleteFromDB")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(PatientModel model)
        {
            unitOfWork.PatientRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
