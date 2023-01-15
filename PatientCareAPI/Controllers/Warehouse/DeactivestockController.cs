using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PatientCareAPI.DataAccess;
using PatientCareAPI.Models.Application;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Models.Warehouse;
using PatientCareAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Controllers.Warehouse
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeactivestockController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<DeactivestockController> _logger;
        private readonly ApplicationDBContext _context;
        Utilities Utilities;
        UnitOfWork unitOfWork;
        public DeactivestockController(IConfiguration configuration, ILogger<DeactivestockController> logger, ApplicationDBContext context)
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
            List<DeactivestockModel> Data = unitOfWork.DeactivestockRepository.GetAll();
            foreach (var item in Data)
            {
                item.Activestock = unitOfWork.StockRepository.GetStockByGuid(item.Activestockid);
                item.Activestock.Stockdefine = unitOfWork.StockdefineRepository.GetStockByGuid(item.Activestock.StockdefineID);
                item.Activestock.Department = unitOfWork.DepartmentRepository.GetDepartmentByGuid(item.Activestock.Departmentid);
            }
            if (Data.Count == 0)
                return NotFound();
            return Ok(Data);
        }
    }
}
