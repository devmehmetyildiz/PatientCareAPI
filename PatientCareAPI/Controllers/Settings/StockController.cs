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
    public class StockController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<StockController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;
        Utilities Utilities;
        public StockController(IConfiguration configuration, ILogger<StockController> logger, ApplicationDBContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
            Utilities = new Utilities(context);
            unitOfWork = new UnitOfWork(context);
        }

        [HttpGet]
        [Authorize(Roles = UserAuthory.Stock_Screen)]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            List<StockModel> Data = new List<StockModel>();
            if (Utilities.CheckAuth(UserAuthory.Stock_ManageAll, this.User.Identity))
            {
                Data = unitOfWork.StockRepository.GetAll().Where(u => u.IsActive).ToList();
                foreach (var item in Data)
                {
                    item.Department = unitOfWork.DepartmentRepository.GetDepartmentByGuid(item.Departmentid);
                    item.Unit = unitOfWork.UnitRepository.GetUnitByGuid(item.Unitid);
                }
            }
            else
            {
                Data = unitOfWork.StockRepository.GetAll().Where(u => u.IsActive && u.CreatedUser == this.User.Identity.Name).ToList();
                foreach (var item in Data)
                {
                    item.Department = unitOfWork.DepartmentRepository.GetDepartmentByGuid(item.Departmentid);
                    item.Unit = unitOfWork.UnitRepository.GetUnitByGuid(item.Unitid);
                }
            }
            if (Data.Count == 0)
            {
                return NotFound();
            }
            return Ok(Data);
        }

        [Route("GetSelectedStock")]
        [Authorize(Roles = (UserAuthory.Stock_Screen + "," + UserAuthory.Stock_Update))]
        [HttpGet]
        public IActionResult GetSelectedStock(int ID)
        {
            StockModel Data = unitOfWork.StockRepository.Getbyid(ID);
            Data.Department= unitOfWork.DepartmentRepository.GetDepartmentByGuid(Data.Departmentid);
            Data.Unit = unitOfWork.UnitRepository.GetUnitByGuid(Data.Unitid);
            if (!Utilities.CheckAuth(UserAuthory.Stock_ManageAll, this.User.Identity))
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
        [Authorize(Roles = UserAuthory.Stock_Add)]
        [HttpPost]
        public IActionResult Add(StockModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            model.Departmentid = model.Department.ConcurrencyStamp;
            model.Unitid = model.Department.ConcurrencyStamp;
            unitOfWork.StockRepository.Add(model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Update")]
        [Authorize(Roles = UserAuthory.Stock_Update)]
        [HttpPost]
        public IActionResult Update(StockModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            if (!Utilities.CheckAuth(UserAuthory.Stock_ManageAll, this.User.Identity))
            {
                if (model.CreatedUser == this.User.Identity.Name)
                {
                    return StatusCode(403);
                }
            }
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            model.Departmentid = model.Department.ConcurrencyStamp;
            model.Unitid = model.Department.ConcurrencyStamp;
            unitOfWork.StockRepository.update(unitOfWork.StockRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Delete")]
        [Authorize(Roles = UserAuthory.Stock_Delete)]
        [HttpDelete]
        public IActionResult Delete(StockModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            if (!Utilities.CheckAuth(UserAuthory.File_ManageAll, this.User.Identity))
            {
                if (model.CreatedUser == this.User.Identity.Name)
                {
                    return StatusCode(403);
                }
            }
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.StockRepository.update(unitOfWork.StockRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("DeleteFromDB")]
        [Authorize(Roles = UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(StockModel model)
        {
            unitOfWork.StockRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
