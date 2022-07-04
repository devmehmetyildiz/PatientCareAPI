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

        [HttpGet]
        [Authorize(Roles = UserAuthory.Unit_Screen)]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            List<UnitModel> Data = new List<UnitModel>();
            if (Utilities.CheckAuth(UserAuthory.Unit_ManageAll, this.User.Identity))
            {
                Data = unitOfWork.UnitRepository.GetAll().Where(u => u.IsActive).ToList();
                foreach (var item in Data)
                {
                    List<string> Departments = unitOfWork.UnittodepartmentRepository.GetAll().Where(u => u.UnitId == item.ConcurrencyStamp).Select(u => u.DepartmentId).ToList();
                    foreach (var department in Departments)
                    {
                        item.Departments.Add(unitOfWork.DepartmentRepository.GetDepartmentByGuid(department));
                    }
                }
            }
            else
            {
                Data = unitOfWork.UnitRepository.GetAll().Where(u => u.IsActive && u.CreatedUser == this.User.Identity.Name).ToList();
                foreach (var item in Data)
                {
                    List<string> Departments = unitOfWork.UnittodepartmentRepository.GetAll().Where(u => u.UnitId == item.ConcurrencyStamp).Select(u => u.DepartmentId).ToList();
                    foreach (var department in Departments)
                    {
                        item.Departments.Add(unitOfWork.DepartmentRepository.GetDepartmentByGuid(department));
                    }
                }
            }
            if (Data.Count == 0)
            {
                return NotFound();
            }
            return Ok(Data);
        }

        [Route("GetSelectedUnit")]
        [Authorize(Roles = (UserAuthory.Unit_Screen + "," +UserAuthory.Unit_Update))]
        [HttpGet]
        public IActionResult GetSelectedUnit(int ID)
        {
            UnitModel Data = unitOfWork.UnitRepository.Getbyid(ID);
            List<string> Departments = unitOfWork.UnittodepartmentRepository.GetAll().Where(u => u.UnitId == Data.ConcurrencyStamp).Select(u => u.DepartmentId).ToList();
            foreach (var department in Departments)
            {
                Data.Departments.Add(unitOfWork.DepartmentRepository.GetDepartmentByGuid(department));
            }
            if (!Utilities.CheckAuth(UserAuthory.Unit_ManageAll, this.User.Identity))
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
        [Authorize(Roles = UserAuthory.Unit_Add)]
        [HttpPost]
        public IActionResult Add(UnitModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.UnitRepository.Add(model);
            unitOfWork.UnittodepartmentRepository.AddDepartments(model.Departments, model.ConcurrencyStamp);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Update")]
        [Authorize(Roles = UserAuthory.Unit_Update)]
        [HttpPost]
        public IActionResult Update(UnitModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            if (!Utilities.CheckAuth(UserAuthory.Unit_ManageAll, this.User.Identity))
            {
                if (model.CreatedUser == this.User.Identity.Name)
                {
                    return StatusCode(403);
                }
            }
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.UnitRepository.update(unitOfWork.UnitRepository.Getbyid(model.Id), model);
            unitOfWork.UnittodepartmentRepository.DeleteDepartmentsByUnit(model.ConcurrencyStamp);
            unitOfWork.UnittodepartmentRepository.AddDepartments(model.Departments, model.ConcurrencyStamp);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Delete")]
        [Authorize(Roles = UserAuthory.Unit_Delete)]
        [HttpDelete]
        public IActionResult Delete(UnitModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            if (!Utilities.CheckAuth(UserAuthory.Unit_ManageAll, this.User.Identity))
            {
                if (model.CreatedUser == this.User.Identity.Name)
                {
                    return StatusCode(403);
                }
            }
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.UnitRepository.update(unitOfWork.UnitRepository.Getbyid(model.Id), model);
            unitOfWork.UnittodepartmentRepository.DeleteDepartmentsByUnit(model.ConcurrencyStamp);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("DeleteFromDB")]
        [Authorize(Roles = UserAuthory.Admin)]
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
