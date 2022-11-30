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
        [AuthorizeMultiplePolicy(UserAuthory.Stock_Screen)]
        [Route("GetAllSettings")]
        public IActionResult GetAllSettings()
        {
            List<StockModel> Data = new List<StockModel>();
            if (Utilities.CheckAuth(UserAuthory.Stock_ManageAll, this.User.Identity))
            {
                Data = unitOfWork.StockRepository.GetAll().Where(u => u.IsActive).ToList();
                foreach (var item in Data)
                {
                    item.Department = unitOfWork.DepartmentRepository.GetDepartmentByGuid(item.Departmentid);
                    item.Station = unitOfWork.StationsRepository.GetStationbyGuid(item.Stationtid);
                    item.Unit = unitOfWork.UnitRepository.GetUnitByGuid(item.Unitid);
                    item.Departmenttxt = item.Department.Name;
                    item.Unittxt = item.Unit.Name;
                    item.Stationtxt= item.Station.Name;
                }
            }
            else
            {
                Data = unitOfWork.StockRepository.GetAll().Where(u => u.IsActive && u.CreatedUser == this.User.Identity.Name).ToList();
                foreach (var item in Data)
                {
                    item.Department = unitOfWork.DepartmentRepository.GetDepartmentByGuid(item.Departmentid);
                    item.Station = unitOfWork.StationsRepository.GetStationbyGuid(item.Stationtid);
                    item.Unit = unitOfWork.UnitRepository.GetUnitByGuid(item.Unitid);
                    item.Departmenttxt = item.Department.Name;
                    item.Stationtxt = item.Station.Name;
                    item.Unittxt = item.Unit.Name;
                }
            }
            if (Data.Count == 0)
            {
                return NotFound();
            }
            return Ok(Data);
        }

        [HttpGet]
        [AuthorizeMultiplePolicy(UserAuthory.Stock_Screen)]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var username = (this.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name)?.Value;
            List<StockModel> Data = new List<StockModel>();
            if (Utilities.CheckAuth(UserAuthory.Stock_ManageAll, this.User.Identity))
            {
                Data = unitOfWork.StockRepository.GetByUserDepartment(username).Where(u => u.IsActive).ToList();
                foreach (var item in Data)
                {
                    item.Department = unitOfWork.DepartmentRepository.GetDepartmentByGuid(item.Departmentid);
                    item.Station = unitOfWork.StationsRepository.GetStationbyGuid(item.Stationtid);
                    item.Unit = unitOfWork.UnitRepository.GetUnitByGuid(item.Unitid);
                    item.Departmenttxt = item.Department.Name;
                    item.Unittxt = item.Unit.Name;
                    item.Stationtxt = item.Station.Name;
                }
            }
            else
            {
                Data = unitOfWork.StockRepository.GetByUserDepartment(username).Where(u => u.IsActive && u.CreatedUser == this.User.Identity.Name).ToList();
                foreach (var item in Data)
                {
                    item.Department = unitOfWork.DepartmentRepository.GetDepartmentByGuid(item.Departmentid);
                    item.Station = unitOfWork.StationsRepository.GetStationbyGuid(item.Stationtid);
                    item.Unit = unitOfWork.UnitRepository.GetUnitByGuid(item.Unitid);
                    item.Departmenttxt = item.Department.Name;
                    item.Stationtxt = item.Station.Name;
                    item.Unittxt = item.Unit.Name;
                }
            }
            if (Data.Count == 0)
            {
                return NotFound();
            }
            return Ok(Data);
        }

        [Route("GetSelectedStock")]
        [AuthorizeMultiplePolicy((UserAuthory.Stock_Screen + "," + UserAuthory.Stock_Update))]
        [HttpGet]
        public IActionResult GetSelectedStock(int ID)
        {
            StockModel Data = unitOfWork.StockRepository.Getbyid(ID);
            Data.Department = unitOfWork.DepartmentRepository.GetDepartmentByGuid(Data.Departmentid);
            Data.Station = unitOfWork.StationsRepository.GetStationbyGuid(Data.Stationtid);
            Data.Unit = unitOfWork.UnitRepository.GetUnitByGuid(Data.Unitid);
            Data.Departmenttxt = Data.Department.Name;
            Data.Stationtxt = Data.Station.Name;
            Data.Unittxt = Data.Unit.Name;
            if (!Utilities.CheckAuth(UserAuthory.Stock_ManageAll, this.User.Identity))
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
        [AuthorizeMultiplePolicy(UserAuthory.Stock_Add)]
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
            model.Stationtid = model.Station.ConcurrencyStamp;
            model.Unitid = model.Unit.ConcurrencyStamp;
            unitOfWork.StockRepository.Add(model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy(UserAuthory.Stock_Update)]
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
            model.Stationtid = model.Station.ConcurrencyStamp;
            model.Unitid = model.Unit.ConcurrencyStamp;
            unitOfWork.StockRepository.update(unitOfWork.StockRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Delete")]
        [AuthorizeMultiplePolicy(UserAuthory.Stock_Delete)]
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
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(StockModel model)
        {
            unitOfWork.StockRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
