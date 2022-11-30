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
    public class PatientreportController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<PatientreportController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;
        Utilities Utilities;
        public PatientreportController(IConfiguration configuration, ILogger<PatientreportController> logger, ApplicationDBContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
            Utilities = new Utilities(context);
            unitOfWork = new UnitOfWork(context);
        }

        #region GetAll
        [Route("GetApplicant")]
        [AuthorizeMultiplePolicy((UserAuthory.Patients_Screen + "," + UserAuthory.Patients_Update))]
        [HttpGet]
        public IActionResult GetApplicant(string Guid)
        {
            PatientapplicantModel Data = unitOfWork.PatientapplicantRepository.GetDatabyGuid(Guid);
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

        [Route("GetBodycontrolform")]
        [AuthorizeMultiplePolicy((UserAuthory.Patients_Screen + "," + UserAuthory.Patients_Update))]
        [HttpGet]
        public IActionResult GetBodycontrolform(string Guid)
        {
            return Ok(unitOfWork.PatientbodycontrolformRepository.GetDataByGuid(Guid));
        }

        [Route("GetDisabilitypermitform")]
        [AuthorizeMultiplePolicy((UserAuthory.Patients_Screen + "," + UserAuthory.Patients_Update))]
        [HttpGet]
        public IActionResult GetDisabilitypermitform(string Guid)
        {
            return Ok(unitOfWork.PatientdisabilitypermitformRepository.GetDataByGuid(Guid));
        }

        [Route("GetDisabledhealthboardreport")]
        [AuthorizeMultiplePolicy((UserAuthory.Patients_Screen + "," + UserAuthory.Patients_Update))]
        [HttpGet]
        public IActionResult Getdisabledhealthboardreport(string Guid)
        {
            return Ok(unitOfWork.PatientdisabledhealthboardreportRepository.GetDataByGuid(Guid));
        }

        [Route("GetFirstadmissionform")]
        [AuthorizeMultiplePolicy((UserAuthory.Patients_Screen + "," + UserAuthory.Patients_Update))]
        [HttpGet]
        public IActionResult Getfirstadmissionform(string Guid)
        {
            return Ok(unitOfWork.PatientfirstadmissionformRepository.GetDataByGuid(Guid));
        }

        [Route("GetFirstapproachreport")]
        [AuthorizeMultiplePolicy((UserAuthory.Patients_Screen + "," + UserAuthory.Patients_Update))]
        [HttpGet]
        public IActionResult Getfirstapproachreport(string Guid)
        {
            return Ok(unitOfWork.PatientfirstapproachreportRepository.GetDataByGuid(Guid));
        }

        [Route("GetOwnershiprecieve")]
        [AuthorizeMultiplePolicy((UserAuthory.Patients_Screen + "," + UserAuthory.Patients_Update))]
        [HttpGet]
        public IActionResult Getownershiprecieve(string Guid)
        {
            return Ok(unitOfWork.PatientownershiprecieveRepository.GetDataByGuid(Guid));
        }

        [Route("GetRecieveform")]
        [AuthorizeMultiplePolicy((UserAuthory.Patients_Screen + "," + UserAuthory.Patients_Update))]
        [HttpGet]
        public IActionResult Getrecieveform(string Guid)
        {
            return Ok(unitOfWork.PatientrecieveformRepository.GetDataByGuid(Guid));
        }

        [Route("GetSubmittingform")]
        [AuthorizeMultiplePolicy((UserAuthory.Patients_Screen + "," + UserAuthory.Patients_Update))]
        [HttpGet]
        public IActionResult GetSubmittingform(string Guid)
        {
            return Ok(unitOfWork.PatientsubmittingformRepository.GetDataByGuid(Guid));
        }
        #endregion

        #region Update
        [Route("UpdateApplicant")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Update)]
        [HttpPost]
        public IActionResult UpdateApplicant(PatientapplicantModel model)
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
            unitOfWork.PatientapplicantRepository.update(unitOfWork.PatientapplicantRepository.GetSingleRecord<PatientapplicantModel>(u=>u.ConcurrencyStamp==model.ConcurrencyStamp), model);
            unitOfWork.Complate();
            return Ok();
        }
        [Route("UpdateBodycontrolform")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Update)]
        [HttpPost]
        public IActionResult UpdateBodycontrolform(PatientbodycontrolformModel model)
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
            unitOfWork.PatientbodycontrolformRepository.update(unitOfWork.PatientbodycontrolformRepository.GetSingleRecord<PatientbodycontrolformModel>(u=>u.ConcurrencyStamp==model.ConcurrencyStamp), model);
            unitOfWork.Complate();
            return Ok();
        }
        [Route("UpdateDisabilitypermitform")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Update)]
        [HttpPost]
        public IActionResult UpdateDisabilitypermitform(PatientdisabilitypermitformModel model)
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
            unitOfWork.PatientdisabilitypermitformRepository.update(unitOfWork.PatientdisabilitypermitformRepository.GetSingleRecord<PatientdisabilitypermitformModel>(u => u.ConcurrencyStamp == model.ConcurrencyStamp), model);
            unitOfWork.Complate();
            return Ok();
        }
        [Route("UpdateDisabledhealthboardreport")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Update)]
        [HttpPost]
        public IActionResult UpdateDisabledhealthboardreport(PatientdisabledhealthboardreportModel model)
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
            unitOfWork.PatientdisabledhealthboardreportRepository.update(unitOfWork.PatientdisabledhealthboardreportRepository.GetSingleRecord<PatientdisabledhealthboardreportModel>(u => u.ConcurrencyStamp == model.ConcurrencyStamp), model);
            unitOfWork.Complate();
            return Ok();
        }
        [Route("UpdateFirstadmissionform")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Update)]
        [HttpPost]
        public IActionResult UpdateFirstadmissionform(PatientfirstadmissionformModel model)
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
            unitOfWork.PatientfirstadmissionformRepository.update(unitOfWork.PatientfirstadmissionformRepository.GetSingleRecord<PatientfirstadmissionformModel>(u => u.ConcurrencyStamp == model.ConcurrencyStamp), model);
            unitOfWork.Complate();
            return Ok();
        }
        [Route("UpdateFirstapproachreport")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Update)]
        [HttpPost]
        public IActionResult UpdateFirstapproachreport(PatientfirstapproachreportModel model)
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
            unitOfWork.PatientfirstapproachreportRepository.update(unitOfWork.PatientfirstapproachreportRepository.GetSingleRecord<PatientfirstapproachreportModel>(u => u.ConcurrencyStamp == model.ConcurrencyStamp), model);
            unitOfWork.Complate();
            return Ok();
        }
        [Route("UpdateOwnershiprecieveModel")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Update)]
        [HttpPost]
        public IActionResult UpdateOwnershiprecieveModel(PatientownershiprecieveModel model)
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
            unitOfWork.PatientownershiprecieveRepository.update(unitOfWork.PatientownershiprecieveRepository.GetSingleRecord<PatientownershiprecieveModel>(u => u.ConcurrencyStamp == model.ConcurrencyStamp), model);
            unitOfWork.Complate();
            return Ok();
        }
        [Route("UpdateRecieveform")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Update)]
        [HttpPost]
        public IActionResult UpdateRecieveform(PatientrecieveformModel model)
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
            unitOfWork.PatientrecieveformRepository.update(unitOfWork.PatientrecieveformRepository.GetSingleRecord<PatientrecieveformModel>(u => u.ConcurrencyStamp == model.ConcurrencyStamp), model);
            unitOfWork.Complate();
            return Ok();
        }
        [Route("UpdatSubmittingform")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Update)]
        [HttpPost]
        public IActionResult UpdatSubmittingform(PatientsubmittingformModel model)
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
            unitOfWork.PatientsubmittingformRepository.update(unitOfWork.PatientsubmittingformRepository.GetSingleRecord<PatientsubmittingformModel>(u => u.ConcurrencyStamp == model.ConcurrencyStamp), model);
            unitOfWork.Complate();
            return Ok();
        }
        #endregion
    }
}
