using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PatientCareAPI.DataAccess;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Models.System;
using PatientCareAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PatientCareAPI.Controllers.System
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailsettingController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<MailsettingController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;
        Utilities Utilities;
        public MailsettingController(IConfiguration configuration, ILogger<MailsettingController> logger, ApplicationDBContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
            Utilities = new Utilities(context);
            unitOfWork = new UnitOfWork(context);
        }
        private string GetSessionUser()
        {
            return (this.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name)?.Value;
        }

        private List<MailsettingModel> FetchList()
        {
            return unitOfWork.MailsettingRepository.GetRecords<MailsettingModel>(u => u.IsActive);
        }

        [HttpGet]
        [AuthorizeMultiplePolicy(UserAuthory.Mailsetting_Screen)]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(FetchList());
        }

        [Route("GetSelected")]
        [AuthorizeMultiplePolicy(UserAuthory.Mailsetting_Getselected)]
        [HttpGet]
        public IActionResult GetSelectedCase(string guid)
        {
            var Data = unitOfWork.MailsettingRepository.GetRecord<MailsettingModel>(u => u.ConcurrencyStamp == guid);
            return Ok(Data);
        }

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Mailsetting_Add)]
        [HttpPost]
        public IActionResult Add(MailsettingModel model)
        {
            var username = GetSessionUser();
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.MailsettingRepository.Add(model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy(UserAuthory.Mailsetting_Edit)]
        [HttpPost]
        public IActionResult Update(MailsettingModel model)
        {
            var username = GetSessionUser();
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            if (model.Issettingactive)
            {
                var list = unitOfWork.MailsettingRepository.GetRecords<MailsettingModel>(u=>u.IsActive && u.ConcurrencyStamp!=model.ConcurrencyStamp);
                foreach (var item in list)
                {
                    item.Issettingactive = false;
                    unitOfWork.MailsettingRepository.update(unitOfWork.MailsettingRepository.Getbyid(item.Id), item);
                }
            }
            unitOfWork.MailsettingRepository.update(unitOfWork.MailsettingRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Delete")]
        [AuthorizeMultiplePolicy(UserAuthory.Mailsetting_Delete)]
        [HttpPost]
        public IActionResult Delete(MailsettingModel model)
        {
            var username = GetSessionUser();
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.MailsettingRepository.update(unitOfWork.MailsettingRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("DeleteFromDB")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(MailsettingModel model)
        {
            unitOfWork.CaseRepository.Remove(model.Id);
            unitOfWork.CasetodepartmentRepository.DeleteDepartmentsByCase(model.ConcurrencyStamp);
            unitOfWork.Complate();
            return Ok();
        }

    }
}
