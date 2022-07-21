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

        [HttpGet]
        [Authorize(Roles = UserAuthory.Costumertype_Screen)]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            List<CostumertypeModel> Data = new List<CostumertypeModel>();
            if (Utilities.CheckAuth(UserAuthory.Costumertype_ManageAll, this.User.Identity))
            {
                Data = unitOfWork.CostumertypeRepository.GetAll().Where(u => u.IsActive).ToList();
                foreach (var item in Data)
                {
                    List<string> Departments = unitOfWork.CostumertypetoDepartmentRepository.GetAll().Where(u => u.CostumertypeID == item.ConcurrencyStamp).Select(u => u.DepartmentID).ToList();
                    foreach (var department in Departments)
                    {
                        item.Departments.Add(unitOfWork.DepartmentRepository.GetDepartmentByGuid(department));
                    }
                }
            }
            else
            {
                Data = unitOfWork.CostumertypeRepository.GetAll().Where(u => u.IsActive && u.CreatedUser == this.User.Identity.Name).ToList();
                foreach (var item in Data)
                {
                    List<string> Departments = unitOfWork.CostumertypetoDepartmentRepository.GetAll().Where(u => u.CostumertypeID == item.ConcurrencyStamp).Select(u => u.DepartmentID).ToList();
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

        [Route("GetSelectedCostumertype")]
        [Authorize(Roles = (UserAuthory.Costumertype_Screen + "," + UserAuthory.Costumertype_Update))]
        [HttpGet]
        public IActionResult GetSelectedCostumertype(int ID)
        {
            CostumertypeModel Data = unitOfWork.CostumertypeRepository.Getbyid(ID);
            List<string> Departments = unitOfWork.CostumertypetoDepartmentRepository.GetAll().Where(u => u.CostumertypeID == Data.ConcurrencyStamp).Select(u => u.DepartmentID).ToList();
            foreach (var department in Departments)
            {
                Data.Departments.Add(unitOfWork.DepartmentRepository.GetDepartmentByGuid(department));
            }
            if (!Utilities.CheckAuth(UserAuthory.Costumertype_ManageAll, this.User.Identity))
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
        [Authorize(Roles = UserAuthory.Costumertype_Add)]
        [HttpPost]
        public IActionResult Add(CostumertypeModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.CostumertypeRepository.Add(model);
            unitOfWork.CostumertypetoDepartmentRepository.AddDepartments(model.Departments, model.ConcurrencyStamp);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Update")]
        [Authorize(Roles = UserAuthory.Costumertype_Update)]
        [HttpPost]
        public IActionResult Update(CostumertypeModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            if (!Utilities.CheckAuth(UserAuthory.Costumertype_ManageAll, this.User.Identity))
            {
                if (model.CreatedUser == this.User.Identity.Name)
                {
                    return StatusCode(403);
                }
            }
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.CostumertypeRepository.update(unitOfWork.CostumertypeRepository.Getbyid(model.Id), model);
            unitOfWork.CostumertypetoDepartmentRepository.DeleteDepartmentsByCostumertype(model.ConcurrencyStamp);
            unitOfWork.CostumertypetoDepartmentRepository.AddDepartments(model.Departments, model.ConcurrencyStamp);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Delete")]
        [Authorize(Roles = UserAuthory.Costumertype_Delete)]
        [HttpDelete]
        public IActionResult Delete(CostumertypeModel model)
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
            unitOfWork.CostumertypeRepository.update(unitOfWork.CostumertypeRepository.Getbyid(model.Id), model);
            unitOfWork.CostumertypetoDepartmentRepository.DeleteDepartmentsByCostumertype(model.ConcurrencyStamp);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("DeleteFromDB")]
        [Authorize(Roles = UserAuthory.Admin)]
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
