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

    public class CaseController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<CaseController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;
        Utilities Utilities;
        public CaseController(IConfiguration configuration, ILogger<CaseController> logger, ApplicationDBContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
            Utilities = new Utilities(context);
            unitOfWork = new UnitOfWork(context);
        }

        [HttpGet]
      //  [Authorize(Roles = UserAuthory.Case_Screen)]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            List<CaseModel> Data = new List<CaseModel>();
            if(Utilities.CheckAuth(UserAuthory.Case_ManageAll,this.User.Identity))
            {
                Data = unitOfWork.CaseRepository.GetAll().Where(u => u.IsActive).ToList();
            }
            else
            {
                Data = unitOfWork.CaseRepository.GetAll().Where(u => u.IsActive && u.CreatedUser == this.User.Identity.Name).ToList();
            }
            if (Data.Count == 0)
            {
                return NotFound();
            }
            return  Ok(Data);
        }

        [Route("GetSelectedCase")]
        [HttpGet]
        public IActionResult GetSelectedCase(int ID)
        {
            CaseModel Data = unitOfWork.CaseRepository.Getbyid(ID);
            if (!Utilities.CheckAuth(UserAuthory.Case_ManageAll, this.User.Identity))
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
        [HttpPost]
        public IActionResult Add(CaseModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.CaseRepository.Add(model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Update")]
        [HttpPost]
        public IActionResult Update(CaseModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            if (!Utilities.CheckAuth(UserAuthory.Case_ManageAll, this.User.Identity))
            {
                if (model.CreatedUser == this.User.Identity.Name)
                {
                    return StatusCode(403);
                }
            }
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.CaseRepository.update(unitOfWork.CaseRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Delete")]
        [HttpDelete]
        public IActionResult Delete(CaseModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            if (!Utilities.CheckAuth(UserAuthory.Case_ManageAll, this.User.Identity))
            {
                if (model.CreatedUser == this.User.Identity.Name)
                {
                    return StatusCode(403);
                }
            }
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;           
            unitOfWork.CaseRepository.update(unitOfWork.CaseRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("DeleteFromDB")]
        [HttpDelete]
        public IActionResult DeleteFromDB(CaseModel model)
        {
            unitOfWork.CaseRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }

    }
}
