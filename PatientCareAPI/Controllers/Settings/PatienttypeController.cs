using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PatientCareAPI.DataAccess;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using PatientCareAPI.Utils;

namespace PatientCareAPI.Controllers.Settings
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PatienttypeController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<PatienttypeController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;
        Utilities Utilities;
        public PatienttypeController(IConfiguration configuration, ILogger<PatienttypeController> logger, ApplicationDBContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
            Utilities = new Utilities(context);
            unitOfWork = new UnitOfWork(context);
        }

        [HttpGet]
        [Authorize(Roles = UserAuthory.Patienttype_Screen)]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            List<PatienttypeModel> Data = new List<PatienttypeModel>();
            if (Utilities.CheckAuth(UserAuthory.Patienttype_ManageAll, this.User.Identity))
            {
                Data = unitOfWork.PatienttypeRepository.GetAll().Where(u => u.IsActive).ToList();
            }
            else
            {
                Data = unitOfWork.PatienttypeRepository.GetAll().Where(u => u.IsActive && u.CreatedUser == this.User.Identity.Name).ToList();
            }
            if (Data.Count == 0)
            {
                return NotFound();
            }
            return Ok(Data);
        }

        [Route("GetSelectedPatienttype")]
        [Authorize(Roles = (UserAuthory.Patienttype_Screen + "," + UserAuthory.Patienttype_Update))]
        [HttpGet]
        public IActionResult GetSelectedPatienttype(int ID)
        {
            PatienttypeModel Data = unitOfWork.PatienttypeRepository.Getbyid(ID);
            if (!Utilities.CheckAuth(UserAuthory.Patienttype_ManageAll, this.User.Identity))
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
        [Authorize(Roles = UserAuthory.Patienttype_Add)]
        [HttpPost]
        public IActionResult Add(PatienttypeModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.PatienttypeRepository.Add(model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Update")]
        [Authorize(Roles = UserAuthory.Patienttype_Update)]
        [HttpPost]
        public IActionResult Update(PatienttypeModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            if (!Utilities.CheckAuth(UserAuthory.Patienttype_ManageAll, this.User.Identity))
            {
                if (model.CreatedUser == this.User.Identity.Name)
                {
                    return StatusCode(403);
                }
            }
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.PatienttypeRepository.update(unitOfWork.PatienttypeRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Delete")]
        [Authorize(Roles = UserAuthory.Patienttype_Delete)]
        [HttpDelete]
        public IActionResult Delete(PatienttypeModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            if (!Utilities.CheckAuth(UserAuthory.Patienttype_ManageAll, this.User.Identity))
            {
                if (model.CreatedUser == this.User.Identity.Name)
                {
                    return StatusCode(403);
                }
            }
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.PatienttypeRepository.update(unitOfWork.PatienttypeRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("DeleteFromDB")]
        [Authorize(Roles = UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(PatienttypeModel model)
        {
            unitOfWork.PatienttypeRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
