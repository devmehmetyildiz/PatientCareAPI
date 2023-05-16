using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PatientCareAPI.DataAccess;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Models.Settings;
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
    public class PrinttemplateController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<PrinttemplateController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;
        Utilities Utilities;
        public PrinttemplateController(IConfiguration configuration, ILogger<PrinttemplateController> logger, ApplicationDBContext context)
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

        private List<PrinttemplateModel> FetchList()
        {
            var list = unitOfWork.PrinttemplateRepository.GetRecords<PrinttemplateModel>(u => u.IsActive);
            foreach (var item in list)
            {
                item.Department = unitOfWork.DepartmentRepository.GetRecord<DepartmentModel>(u => u.IsActive && u.ConcurrencyStamp == item.DepartmentID);
            }
            return list;
        }

        [HttpGet]
        [AuthorizeMultiplePolicy(UserAuthory.Printtemplate_Screen)]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(FetchList());
        }

        [Route("GetSelected")]
        [AuthorizeMultiplePolicy(UserAuthory.Printtemplate_Getselected )]
        [HttpGet]
        public IActionResult GetSelectedCase(string guid)
        {
            var Data = unitOfWork.PrinttemplateRepository.GetRecord<PrinttemplateModel>(u => u.ConcurrencyStamp == guid);
            Data.Department = unitOfWork.DepartmentRepository.GetRecord<DepartmentModel>(u => u.IsActive && u.ConcurrencyStamp == Data.DepartmentID);
            return Ok(Data);
        }

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Printtemplate_Add)]
        [HttpPost]
        public IActionResult Add(PrinttemplateModel model)
        {
            var username = GetSessionUser();
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.PrinttemplateRepository.Add(model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy(UserAuthory.Printtemplate_Edit)]
        [HttpPost]
        public IActionResult Update(PrinttemplateModel model)
        {
            var username = GetSessionUser();
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.PrinttemplateRepository.update(unitOfWork.PrinttemplateRepository.GetRecord<PrinttemplateModel>(u=>u.ConcurrencyStamp==model.ConcurrencyStamp), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Delete")]
        [AuthorizeMultiplePolicy(UserAuthory.Printtemplate_Delete)]
        [HttpPost]
        public IActionResult Delete(PrinttemplateModel model)
        {
            var username = GetSessionUser();
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.PrinttemplateRepository.update(unitOfWork.PrinttemplateRepository.GetRecord<PrinttemplateModel>(u => u.ConcurrencyStamp == model.ConcurrencyStamp), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("DeleteFromDB")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(PrinttemplateModel model)
        {
            unitOfWork.PrinttemplateRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }

       


    }
}
