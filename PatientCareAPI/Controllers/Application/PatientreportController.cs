﻿using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = (UserAuthory.Patients_Screen + "," + UserAuthory.Patients_Update))]
        [HttpGet]
        public IActionResult GetApplicant(string Guid)
        {
            PatientapplicantModel Data = unitOfWork.PatientapplicantRepository.GetByGuid(Guid);
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
        [Authorize(Roles = (UserAuthory.Patients_Screen + "," + UserAuthory.Patients_Update))]
        [HttpGet]
        public IActionResult GetBodycontrolform(string Guid)
        {
            PatientbodycontrolformModel Data = unitOfWork.PatientbodycontrolformRepository.GetByGuid(Guid);
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

        [Route("GetDisabilitypermitform")]
        [Authorize(Roles = (UserAuthory.Patients_Screen + "," + UserAuthory.Patients_Update))]
        [HttpGet]
        public IActionResult GetDisabilitypermitform(string Guid)
        {
            PatientdisabilitypermitformModel Data = unitOfWork.PatientdisabilitypermitformRepository.GetByGuid(Guid);
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

        [Route("GetDisabledhealthboardreport")]
        [Authorize(Roles = (UserAuthory.Patients_Screen + "," + UserAuthory.Patients_Update))]
        [HttpGet]
        public IActionResult Getdisabledhealthboardreport(string Guid)
        {
            PatientdisabledhealthboardreportModel Data = unitOfWork.PatientdisabledhealthboardreportRepository.GetByGuid(Guid);
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

        [Route("GetFirstadmissionform")]
        [Authorize(Roles = (UserAuthory.Patients_Screen + "," + UserAuthory.Patients_Update))]
        [HttpGet]
        public IActionResult Getfirstadmissionform(string Guid)
        {
            PatientfirstadmissionformModel Data = unitOfWork.PatientfirstadmissionformRepository.GetByGuid(Guid);
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

        [Route("GetFirstapproachreport")]
        [Authorize(Roles = (UserAuthory.Patients_Screen + "," + UserAuthory.Patients_Update))]
        [HttpGet]
        public IActionResult Getfirstapproachreport(string Guid)
        {
            PatientfirstapproachreportModel Data = unitOfWork.PatientfirstapproachreportRepository.GetByGuid(Guid);
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

        [Route("GetOwnershiprecieve")]
        [Authorize(Roles = (UserAuthory.Patients_Screen + "," + UserAuthory.Patients_Update))]
        [HttpGet]
        public IActionResult Getownershiprecieve(string Guid)
        {
            PatientownershiprecieveModel Data = unitOfWork.PatientownershiprecieveRepository.GetByGuid(Guid);
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

        [Route("GetRecieveform")]
        [Authorize(Roles = (UserAuthory.Patients_Screen + "," + UserAuthory.Patients_Update))]
        [HttpGet]
        public IActionResult Getrecieveform(string Guid)
        {
            PatientrecieveformModel Data = unitOfWork.PatientrecieveformRepository.GetByGuid(Guid);
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

        [Route("GetSubmittingform")]
        [Authorize(Roles = (UserAuthory.Patients_Screen + "," + UserAuthory.Patients_Update))]
        [HttpGet]
        public IActionResult GetSubmittingform(string Guid)
        {
            PatientsubmittingformModel Data = unitOfWork.PatientsubmittingformRepository.GetByGuid(Guid);
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
        #endregion

        #region Update
        [Route("UpdateApplicant")]
        [Authorize(Roles = UserAuthory.Patients_Update)]
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
            unitOfWork.PatientapplicantRepository.update(unitOfWork.PatientapplicantRepository.GetByGuid(model.ConcurrencyStamp), model);
            unitOfWork.Complate();
            return Ok();
        }
        [Route("UpdateBodycontrolform")]
        [Authorize(Roles = UserAuthory.Patients_Update)]
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
            unitOfWork.PatientbodycontrolformRepository.update(unitOfWork.PatientbodycontrolformRepository.GetByGuid(model.ConcurrencyStamp), model);
            unitOfWork.Complate();
            return Ok();
        }
        [Route("UpdateDisabilitypermitform")]
        [Authorize(Roles = UserAuthory.Patients_Update)]
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
            unitOfWork.PatientdisabilitypermitformRepository.update(unitOfWork.PatientdisabilitypermitformRepository.GetByGuid(model.ConcurrencyStamp), model);
            unitOfWork.Complate();
            return Ok();
        }
        [Route("UpdateDisabledhealthboardreport")]
        [Authorize(Roles = UserAuthory.Patients_Update)]
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
            unitOfWork.PatientdisabledhealthboardreportRepository.update(unitOfWork.PatientdisabledhealthboardreportRepository.GetByGuid(model.ConcurrencyStamp), model);
            unitOfWork.Complate();
            return Ok();
        }
        [Route("UpdateFirstadmissionform")]
        [Authorize(Roles = UserAuthory.Patients_Update)]
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
            unitOfWork.PatientfirstadmissionformRepository.update(unitOfWork.PatientfirstadmissionformRepository.GetByGuid(model.ConcurrencyStamp), model);
            unitOfWork.Complate();
            return Ok();
        }
        [Route("UpdateFirstapproachreport")]
        [Authorize(Roles = UserAuthory.Patients_Update)]
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
            unitOfWork.PatientfirstapproachreportRepository.update(unitOfWork.PatientfirstapproachreportRepository.GetByGuid(model.ConcurrencyStamp), model);
            unitOfWork.Complate();
            return Ok();
        }
        [Route("UpdateOwnershiprecieveModel")]
        [Authorize(Roles = UserAuthory.Patients_Update)]
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
            unitOfWork.PatientownershiprecieveRepository.update(unitOfWork.PatientownershiprecieveRepository.GetByGuid(model.ConcurrencyStamp), model);
            unitOfWork.Complate();
            return Ok();
        }
        [Route("UpdateRecieveform")]
        [Authorize(Roles = UserAuthory.Patients_Update)]
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
            unitOfWork.PatientrecieveformRepository.update(unitOfWork.PatientrecieveformRepository.GetByGuid(model.ConcurrencyStamp), model);
            unitOfWork.Complate();
            return Ok();
        }
        [Route("UpdatSubmittingform")]
        [Authorize(Roles = UserAuthory.Patients_Update)]
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
            unitOfWork.PatientsubmittingformRepository.update(unitOfWork.PatientsubmittingformRepository.GetByGuid(model.ConcurrencyStamp), model);
            unitOfWork.Complate();
            return Ok();
        }
        #endregion
    }
}
