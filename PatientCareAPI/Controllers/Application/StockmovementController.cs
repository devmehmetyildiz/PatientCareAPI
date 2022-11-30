using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PatientCareAPI.DataAccess;
using PatientCareAPI.Models.Application;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Controllers.Application
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockmovementController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<StockmovementController> _logger;
        private readonly ApplicationDBContext _context;
        Utilities Utilities;
        UnitOfWork unitOfWork;
        public StockmovementController(IConfiguration configuration, ILogger<StockmovementController> logger, ApplicationDBContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
            Utilities = new Utilities(context);
            unitOfWork = new UnitOfWork(context);
        }

        [Route("GetAll")]
        [AuthorizeMultiplePolicy(UserAuthory.Stock_Screen)]
        [HttpGet]
        public IActionResult GetAll()
        {
            List<StockmovementModel> Data = new List<StockmovementModel>();
            Data = unitOfWork.StockmovementRepository.GetAll();
            foreach (var item in Data)
            {
                item.Activestock = unitOfWork.ActivestockRepository.GetStockByGuid(item.Activestockid);
                item.Activestock.Stock = unitOfWork.StockRepository.GetStockByGuid(item.Activestock.Stockid);
                item.Activestock.Department = unitOfWork.DepartmentRepository.GetDepartmentByGuid(item.Activestock.Departmentid);
                item.Username = unitOfWork.UsersRepository.GetUsertByGuid(item.UserID).Username;
            }
            if (Data.Count == 0)
                return NotFound();
            return Ok(Data);
        }

        [Route("GetAllSelected")]
        [AuthorizeMultiplePolicy(UserAuthory.Stock_Screen)]
        [HttpGet]
        public IActionResult GetAllSelected(string guid)
        {
            List<StockmovementModel> Data = new List<StockmovementModel>();
            Data = unitOfWork.StockmovementRepository.FindByActivestockGuid(guid);
            foreach (var item in Data)
            {
                item.Activestock = unitOfWork.ActivestockRepository.GetStockByGuid(item.Activestockid);
                item.Activestock.Stock = unitOfWork.StockRepository.GetStockByGuid(item.Activestock.Stockid);
                item.Activestock.Department = unitOfWork.DepartmentRepository.GetDepartmentByGuid(item.Activestock.Departmentid);
                item.Username = unitOfWork.UsersRepository.GetUsertByGuid(item.UserID).Username;
            }
            if (Data.Count == 0)
                return NotFound();
            return Ok(Data);
        }

        [Route("GetSelectedStock")]
        [AuthorizeMultiplePolicy(UserAuthory.Stock_Screen)]
        [HttpGet]
        public IActionResult GetSelectedStock(string guid)
        {
            var Data = unitOfWork.StockRepository.GetStockByGuid(guid);
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
    }
}
