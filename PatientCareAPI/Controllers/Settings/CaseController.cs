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
        [AuthorizeMultiplePolicy(UserAuthory.Case_Screen)]
        [Route("GetAllSettings")]
        public IActionResult GetAllSettings()
        {
            List<CaseModel> Data = new List<CaseModel>();
            if(Utilities.CheckAuth(UserAuthory.Case_ManageAll,this.User.Identity))
            {
                Data = unitOfWork.CaseRepository.GetAll().Where(u => u.IsActive).ToList();
                foreach (var item in Data)
                {
                    List<string> Departments = unitOfWork.CasetodepartmentRepository.GetAll().Where(u => u.CaseID == item.ConcurrencyStamp).Select(u => u.DepartmentID).ToList();
                    foreach (var department in Departments)
                    {
                        item.Departments.Add(unitOfWork.DepartmentRepository.GetDepartmentByGuid(department));
                    }
                }
            }
            else
            {
                Data = unitOfWork.CaseRepository.GetAll().Where(u => u.IsActive && u.CreatedUser == this.User.Identity.Name).ToList();
                foreach (var item in Data)
                {
                    List<string> Departments = unitOfWork.CasetodepartmentRepository.GetAll().Where(u => u.CaseID == item.ConcurrencyStamp).Select(u => u.DepartmentID).ToList();
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
            return  Ok(Data);
        }

        [HttpGet]
        [AuthorizeMultiplePolicy(UserAuthory.Case_Screen)]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var username = (this.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name)?.Value;
            List<CaseModel> Data = new List<CaseModel>();
            if (Utilities.CheckAuth(UserAuthory.Case_ManageAll, this.User.Identity))
            {
                Data = unitOfWork.CaseRepository.GetByUserDepartment(username).Where(u => u.IsActive).ToList();
                foreach (var item in Data)
                {
                    List<string> Departments = unitOfWork.CasetodepartmentRepository.GetAll().Where(u => u.CaseID == item.ConcurrencyStamp).Select(u => u.DepartmentID).ToList();
                    foreach (var department in Departments)
                    {
                        item.Departments.Add(unitOfWork.DepartmentRepository.GetDepartmentByGuid(department));
                    }
                }
            }
            else
            {
                Data = unitOfWork.CaseRepository.GetByUserDepartment(username).Where(u => u.IsActive && u.CreatedUser == this.User.Identity.Name).ToList();
                foreach (var item in Data)
                {
                    List<string> Departments = unitOfWork.CasetodepartmentRepository.GetAll().Where(u => u.CaseID == item.ConcurrencyStamp).Select(u => u.DepartmentID).ToList();
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

        [Route("GetSelectedCase")]
        [AuthorizeMultiplePolicy(UserAuthory.Case_Screen + "," + UserAuthory.Case_Update)]
        [HttpGet]
        public IActionResult GetSelectedCase(int ID)
        {
            CaseModel Data = unitOfWork.CaseRepository.Getbyid(ID);
            List<string> Departments = unitOfWork.CasetodepartmentRepository.GetAll().Where(u => u.CaseID == Data.ConcurrencyStamp).Select(u => u.DepartmentID).ToList();
            foreach (var department in Departments)
            {
                Data.Departments.Add(unitOfWork.DepartmentRepository.GetDepartmentByGuid(department));
            }
            if (!Utilities.CheckAuth(UserAuthory.Case_ManageAll, this.User.Identity))
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

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Case_Add)]
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
            unitOfWork.CasetodepartmentRepository.AddDepartments(model.Departments, model.ConcurrencyStamp);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy(UserAuthory.Case_Update)]
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
            unitOfWork.CasetodepartmentRepository.DeleteDepartmentsByCase(model.ConcurrencyStamp);
            unitOfWork.CasetodepartmentRepository.AddDepartments(model.Departments, model.ConcurrencyStamp);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Delete")]
        [AuthorizeMultiplePolicy(UserAuthory.Case_Delete)]
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
            unitOfWork.CasetodepartmentRepository.DeleteDepartmentsByCase(model.ConcurrencyStamp);
            unitOfWork.Complate();
            return Ok();
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
