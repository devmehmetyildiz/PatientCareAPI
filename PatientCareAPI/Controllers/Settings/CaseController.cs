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
        // TODO: Sadece 1 tane tamamlandı ve 1 tane iptal edildi durumu eklenecek  
        private string GetSessionUser()
        {
            return (this.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name)?.Value;
        }

        private List<CaseModel> FetchList()
        {
            var List = unitOfWork.CaseRepository.GetRecords<CaseModel>(u => u.IsActive);
            foreach (var item in List)
            {
                var departmentguids = unitOfWork.CasetodepartmentRepository.GetRecords<CasetoDepartmentModel>(u => u.CaseID == item.ConcurrencyStamp).Select(u => u.DepartmentID).ToList();
                item.Departments = unitOfWork.DepartmentRepository.GetDepartmentsbyGuids(departmentguids);
            }
            return List;
        }

        [HttpGet]
        [AuthorizeMultiplePolicy(UserAuthory.Case_Screen)]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(FetchList());
        }

        [Route("GetSelected")]
        [AuthorizeMultiplePolicy(UserAuthory.Case_Getselected)]
        [HttpGet]
        public IActionResult GetSelectedCase(string guid)
        {
            var Data = unitOfWork.CaseRepository.GetRecord<CaseModel>(u => u.ConcurrencyStamp == guid);
            if (Data == null)
            {
                return NotFound();
            }
            var departmentguids = unitOfWork.CasetodepartmentRepository.GetRecords<CasetoDepartmentModel>(u => u.CaseID == Data.ConcurrencyStamp).Select(u => u.DepartmentID).ToList();
            Data.Departments = unitOfWork.DepartmentRepository.GetDepartmentsbyGuids(departmentguids);
            return Ok(Data);
        }

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Case_Add)]
        [HttpPost]
        public IActionResult Add(CaseModel model)
        {
            var username = GetSessionUser();
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.CaseRepository.Add(model);
            List<CasetoDepartmentModel> list = new List<CasetoDepartmentModel>();
            foreach (var item in model.Departments)
            {
                list.Add(new CasetoDepartmentModel { CaseID = model.ConcurrencyStamp, DepartmentID = item.ConcurrencyStamp });
            }
            unitOfWork.CasetodepartmentRepository.AddRange(list);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy(UserAuthory.Case_Edit)]
        [HttpPost]
        public IActionResult Update(CaseModel model)
        {
            var username = GetSessionUser();
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.CaseRepository.update(unitOfWork.CaseRepository.Getbyid(model.Id), model);
            unitOfWork.CasetodepartmentRepository.RemoveCasesfromDepartment(model.ConcurrencyStamp);
            List<CasetoDepartmentModel> list = new List<CasetoDepartmentModel>();
            foreach (var item in model.Departments)
            {
                list.Add(new CasetoDepartmentModel { CaseID = model.ConcurrencyStamp, DepartmentID = item.ConcurrencyStamp });
            }
            unitOfWork.CasetodepartmentRepository.AddRange(list);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Delete")]
        [AuthorizeMultiplePolicy(UserAuthory.Case_Delete)]
        [HttpPost]
        public IActionResult Delete(CaseModel model)
        {
            var username = GetSessionUser();
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.CaseRepository.update(unitOfWork.CaseRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("DeleteFromDB")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(CaseModel model)
        {
            unitOfWork.CaseRepository.Remove(model.Id);
            unitOfWork.CasetodepartmentRepository.DeleteDepartmentsByCase(model.ConcurrencyStamp);
            unitOfWork.Complate();
            return Ok();
        }

    }
}
