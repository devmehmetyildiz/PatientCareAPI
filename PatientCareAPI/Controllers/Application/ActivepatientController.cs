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
        [Route("GetSelectedDepartment")]
        [HttpGet]
        public IActionResult GetSelectedCase(int ID)
        {
            ActivepatientModel Data = unitOfWork.ActivepatientRepository.Getbyid(ID);
            Data.Patient = unitOfWork.PatientRepository.GetPatientByGuid(Data.PatientID);
            if (Utilities.CheckAuth(UserAuthory.Patients_ManageAll, this.User.Identity))
            {
                if (Data.CreatedUser == this.User.Identity.Name)
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
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            model.PatientID = model.Patient.ConcurrencyStamp;
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
