using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PatientCareAPI.DataAccess;
using PatientCareAPI.Models.Application;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Models.Settings;
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
            try
            {
                Data = unitOfWork.StockmovementRepository.GetAll();
                foreach (var item in Data)
                {
                    item.Stock = unitOfWork.StockRepository.GetSingleRecord<StockModel>(u => u.ConcurrencyStamp == item.Activestockid);
                    item.Stock.Stockdefine = unitOfWork.StockdefineRepository.GetSingleRecord<StockdefineModel>(u => u.ConcurrencyStamp == item.Stock.Stockid);
                    item.Stock.Stockdefine.Unit = unitOfWork.UnitRepository.GetSingleRecord<UnitModel>(u => u.ConcurrencyStamp == item.Stock.Stockdefine.Unitid);
                    item.Stock.Department = unitOfWork.DepartmentRepository.GetSingleRecord<DepartmentModel>(u => u.ConcurrencyStamp == item.Stock.Stockdefine.Departmentid);
                    item.Username = unitOfWork.UsersRepository.GetUsertByGuid(item.UserID).Username;
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return Ok(Data);
        }

        [Route("GetSelected")]
        [AuthorizeMultiplePolicy(UserAuthory.Stock_Screen)]
        [HttpGet]
        public IActionResult GetAllSelected(string guid)
        {
            List<StockmovementModel> Data = new List<StockmovementModel>();
            try
            {
                Data = unitOfWork.StockmovementRepository.GetRecords<StockmovementModel>(u => u.Activestockid == guid);
                foreach (var item in Data)
                {
                    item.Stock = unitOfWork.StockRepository.GetSingleRecord<StockModel>(u => u.ConcurrencyStamp == item.Activestockid);
                    item.Stock.Stockdefine = unitOfWork.StockdefineRepository.GetSingleRecord<StockdefineModel>(u => u.ConcurrencyStamp == item.Stock.Stockid);
                    item.Stock.Department = unitOfWork.DepartmentRepository.GetSingleRecord<DepartmentModel>(u => u.ConcurrencyStamp == item.Stock.Stockdefine.Departmentid);
                    item.Stock.Stockdefine.Unit = unitOfWork.UnitRepository.GetSingleRecord<UnitModel>(u => u.ConcurrencyStamp == item.Stock.Stockdefine.Unitid);
                    item.Username = unitOfWork.UsersRepository.GetUsertByGuid(item.UserID).Username;
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            return Ok(Data);
        }

    }
}


