using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PatientCareAPI.DataAccess;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Models.Settings;
using PatientCareAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PatientCareAPI.Controllers.Settings
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<UnitController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;
        Utilities Utilities;
        public UnitController(IConfiguration configuration, ILogger<UnitController> logger, ApplicationDBContext context)
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

        private List<UnitModel> FetchList()
        {
            var List = unitOfWork.UnitRepository.GetRecords<UnitModel>(u => u.IsActive);
            foreach (var item in List)
            {
                var departmentguids = unitOfWork.CasetodepartmentRepository.GetRecords<UnittoDepartmentModel>(u => u.UnitId == item.ConcurrencyStamp).Select(u => u.DepartmentId).ToList();
                item.Departments = unitOfWork.DepartmentRepository.GetDepartmentsbyGuids(departmentguids);
            }
            return List;
        }

        [HttpGet]
        [AuthorizeMultiplePolicy(UserAuthory.Unit_Screen)]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(FetchList());
        }

        [Route("GetSelected")]
        [AuthorizeMultiplePolicy((UserAuthory.Unit_Getselected ))]
        [HttpGet]
        public IActionResult GetSelectedUnit(string guid)
        {
            var Data = unitOfWork.UnitRepository.GetRecord<UnitModel>(u => u.ConcurrencyStamp == guid);
            if (Data == null)
            {
                return NotFound();
            }
            var departmentguids = unitOfWork.UnittodepartmentRepository.GetRecords<UnittoDepartmentModel>(u => u.UnitId == Data.ConcurrencyStamp).Select(u => u.DepartmentId).ToList();
            Data.Departments = unitOfWork.DepartmentRepository.GetDepartmentsbyGuids(departmentguids);
            return Ok(Data);
        }

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Unit_Add)]
        [HttpPost]
        public IActionResult Add(UnitModel model)
        {
            var username = GetSessionUser();
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.UnitRepository.Add(model);
            List<UnittoDepartmentModel> list = new List<UnittoDepartmentModel>();
            foreach (var item in model.Departments)
            {
                list.Add(new UnittoDepartmentModel { UnitId = model.ConcurrencyStamp, DepartmentId = item.ConcurrencyStamp });
            }
            unitOfWork.UnittodepartmentRepository.AddRange(list);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy(UserAuthory.Unit_Edit)]
        [HttpPost]
        public IActionResult Update(UnitModel model)
        {
            var username = GetSessionUser();
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.UnitRepository.update(unitOfWork.UnitRepository.Getbyid(model.Id), model);
            unitOfWork.UnittodepartmentRepository.DeleteDepartmentsByUnit(model.ConcurrencyStamp);
            List<UnittoDepartmentModel> list = new List<UnittoDepartmentModel>();
            foreach (var item in model.Departments)
            {
                list.Add(new UnittoDepartmentModel { UnitId = model.ConcurrencyStamp, DepartmentId = item.ConcurrencyStamp });
            }
            unitOfWork.UnittodepartmentRepository.AddRange(list);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Delete")]
        [AuthorizeMultiplePolicy(UserAuthory.Unit_Delete)]
        [HttpPost]
        public IActionResult Delete(UnitModel model)
        {
            var username = GetSessionUser();
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.UnitRepository.update(unitOfWork.UnitRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("DeleteFromDB")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(UnitModel model)
        {
            unitOfWork.UnitRepository.Remove(model.Id);
            unitOfWork.UnittodepartmentRepository.DeleteDepartmentsByUnit(model.ConcurrencyStamp);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
