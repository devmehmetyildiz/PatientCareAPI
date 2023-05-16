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
    public class CostumertypeController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<CostumertypeController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;
        Utilities Utilities;
        public CostumertypeController(IConfiguration configuration, ILogger<CostumertypeController> logger, ApplicationDBContext context)
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

        private List<CostumertypeModel> FetchList()
        {
            var List = unitOfWork.CostumertypeRepository.GetRecords<CostumertypeModel>(u => u.IsActive);
            foreach (var item in List)
            {
                var departmentguids = unitOfWork.CostumertypetoDepartmentRepository.GetRecords<CostumertypetoDepartmentModel>(u => u.CostumertypeID == item.ConcurrencyStamp).Select(u => u.DepartmentID).ToList();
                item.Departments = unitOfWork.DepartmentRepository.GetDepartmentsbyGuids(departmentguids);
            }
            return List;
        }

        [Route("GetAll")]
        [AuthorizeMultiplePolicy(UserAuthory.Costumertype_Screen)]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(FetchList());
        }

        [Route("GetSelected")]
        [AuthorizeMultiplePolicy(UserAuthory.Costumertype_Getselected)]
        [HttpGet]
        public IActionResult GetSelectedStation(string guid)
        {
            var Data = unitOfWork.CostumertypeRepository.GetRecord<CostumertypeModel>(u => u.ConcurrencyStamp == guid);
            if (Data == null)
            {
                return NotFound();
            }
            var departmentguids = unitOfWork.CostumertypetoDepartmentRepository.GetRecords<CostumertypetoDepartmentModel>(u => u.CostumertypeID == Data.ConcurrencyStamp).Select(u => u.DepartmentID).ToList();
            Data.Departments = unitOfWork.DepartmentRepository.GetDepartmentsbyGuids(departmentguids);
            return Ok(Data);
        }

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Costumertype_Add)]
        [HttpPost]
        public IActionResult Add(CostumertypeModel model)
        {
            var username = GetSessionUser();
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.CostumertypeRepository.Add(model);
            List<CostumertypetoDepartmentModel> list = new List<CostumertypetoDepartmentModel>();
            foreach (var item in model.Departments)
            {
                list.Add(new CostumertypetoDepartmentModel { CostumertypeID = model.ConcurrencyStamp, DepartmentID = item.ConcurrencyStamp });
            }
            unitOfWork.CostumertypetoDepartmentRepository.AddRange(list);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy((UserAuthory.Costumertype_Edit))]
        [HttpPost]
        public IActionResult Update(CostumertypeModel model)
        {
            var username = GetSessionUser();
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.CostumertypeRepository.update(unitOfWork.CostumertypeRepository.Getbyid(model.Id), model);
            unitOfWork.CostumertypetoDepartmentRepository.DeleteDepartmentsByCostumertype(model.ConcurrencyStamp);
            List<CostumertypetoDepartmentModel> list = new List<CostumertypetoDepartmentModel>();
            foreach (var item in model.Departments)
            {
                list.Add(new CostumertypetoDepartmentModel { CostumertypeID = model.ConcurrencyStamp, DepartmentID = item.ConcurrencyStamp });
            }
            unitOfWork.CostumertypetoDepartmentRepository.AddRange(list);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Delete")]
        [AuthorizeMultiplePolicy(UserAuthory.Costumertype_Delete)]
        [HttpPost]
        public IActionResult Delete(CostumertypeModel model)
        {
            var username = GetSessionUser();
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.CostumertypeRepository.update(unitOfWork.CostumertypeRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("DeleteFromDB")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(CostumertypeModel model)
        {
            unitOfWork.CostumertypeRepository.Remove(model.Id);
            unitOfWork.CostumertypetoDepartmentRepository.DeleteDepartmentsByCostumertype(model.ConcurrencyStamp);
            unitOfWork.Complate();
            return Ok();
        }

    }
}
