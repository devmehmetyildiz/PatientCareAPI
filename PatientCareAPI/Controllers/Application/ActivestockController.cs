using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PatientCareAPI.DataAccess;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Models.Application;
using PatientCareAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace PatientCareAPI.Controllers.Application
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ActivestockController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<ActivestockController> _logger;
        private readonly ApplicationDBContext _context;
        Utilities Utilities;
        UnitOfWork unitOfWork;
        public ActivestockController(IConfiguration configuration, ILogger<ActivestockController> logger, ApplicationDBContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
            Utilities = new Utilities(context);
            unitOfWork = new UnitOfWork(context);
        }

        [Route("GetAll")]
        [Authorize(Roles = UserAuthory.Stock_Screen)]
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ActivestockModel> Data = new List<ActivestockModel>();
            Data = unitOfWork.ActivestockRepository.GetAll();
            foreach (var item in Data)
            {
                item.Stock = unitOfWork.StockRepository.GetStockByGuid(item.Stockid);
            }
            if (Data.Count == 0)
                return NotFound();
            return Ok(Data);
        }

        [Authorize(Roles = (UserAuthory.Stock_Screen + "," + UserAuthory.Stock_Update))]
        [Route("GetSelectedDepartment")]
        [HttpGet]
        public IActionResult GetSelectedCase(int ID)
        {
            ActivestockModel Data = unitOfWork.ActivestockRepository.Getbyid(ID);
            Data.Stock = unitOfWork.StockRepository.GetStockByGuid(Data.Stockid);
            if (Data == null)
            {
                return NotFound();
            }
            return Ok(Data);
        }

        [Route("Add")]
        [Authorize(Roles = UserAuthory.Stock_Add)]
        [HttpPost]
        public IActionResult Add(ActivestockModel model)
        {
            model.Stockid = model.Stock.ConcurrencyStamp;
            unitOfWork.ActivestockRepository.Add(model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Update")]
        [Authorize(Roles = UserAuthory.Stock_Update)]
        [HttpPost]
        public IActionResult Update(ActivestockModel model)
        {
            model.Stockid = model.Stock.ConcurrencyStamp;
            unitOfWork.ActivestockRepository.update(unitOfWork.ActivestockRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Delete")]
        [Authorize(Roles = UserAuthory.Stock_Delete)]
        [HttpDelete]
        public IActionResult Delete(ActivestockModel model)
        {
            unitOfWork.ActivestockRepository.update(unitOfWork.ActivestockRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("DeleteFromDB")]
        [Authorize(Roles = UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(ActivestockModel model)
        {
            unitOfWork.ActivestockRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
