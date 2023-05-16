using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PatientCareAPI.DataAccess;
using PatientCareAPI.Models.Application;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Models.Settings;
using PatientCareAPI.Models.Warehouse;
using PatientCareAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PatientCareAPI.Controllers.Warehouse
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

        private string GetSessionUser()
        {
            return (this.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name)?.Value;
        }

        private List<StockmovementModel> FetchList()
        {
            var List = unitOfWork.StockmovementRepository.GetRecords<StockmovementModel>(u => u.IsActive);
            foreach (var item in List)
            {
                item.Stock = unitOfWork.StockRepository.GetRecord<StockModel>(u => u.ConcurrencyStamp == item.StockID);
                item.Stock.Stockdefine = unitOfWork.StockdefineRepository.GetRecord<StockdefineModel>(u => u.ConcurrencyStamp == item.Stock.StockdefineID);
                item.Stock.Department = unitOfWork.DepartmentRepository.GetRecord<DepartmentModel>(u => u.ConcurrencyStamp == item.Stock.Departmentid);
                item.Stock.Stockdefine.Unit = unitOfWork.UnitRepository.GetRecord<UnitModel>(u => u.ConcurrencyStamp == item.Stock.Stockdefine.Unitid);
            }
            return List;
        }

        [Route("GetAll")]
        [AuthorizeMultiplePolicy(UserAuthory.Stockmovement_Screen)]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(FetchList());
        }

        [Route("GetSelected")]
        [AuthorizeMultiplePolicy((UserAuthory.Stockmovement_Getselected))]
        [HttpGet]
        public IActionResult GetSelected(string guid)
        {
            StockmovementModel Data = unitOfWork.StockmovementRepository.GetRecord<StockmovementModel>(u => u.ConcurrencyStamp == guid);
            Data.Stock = unitOfWork.StockRepository.GetRecord<StockModel>(u => u.ConcurrencyStamp == Data.StockID);
            Data.Stock.Stockdefine = unitOfWork.StockdefineRepository.GetRecord<StockdefineModel>(u => u.ConcurrencyStamp == Data.Stock.StockdefineID);
            Data.Stock.Department = unitOfWork.DepartmentRepository.GetRecord<DepartmentModel>(u => u.ConcurrencyStamp == Data.Stock.Departmentid);
            Data.Stock.Stockdefine.Unit = unitOfWork.UnitRepository.GetRecord<UnitModel>(u => u.ConcurrencyStamp == Data.Stock.Stockdefine.Unitid);
            return Ok(Data);
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy(UserAuthory.Stockmovement_Edit)]
        [HttpPost]
        public IActionResult Update(StockmovementModel model)
        {
            var username = GetSessionUser();
            StockmovementModel oldmodel = unitOfWork.StockmovementRepository.GetRecord<StockmovementModel>(u => u.ConcurrencyStamp == model.ConcurrencyStamp);
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.StockmovementRepository.update(oldmodel, model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

    }
}


