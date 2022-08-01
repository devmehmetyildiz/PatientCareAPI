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
                item.Department = unitOfWork.DepartmentRepository.GetDepartmentByGuid(item.Departmentid);
            }
            if (Data.Count == 0)
                return NotFound();
            return Ok(Data);
        }

        [Authorize(Roles = (UserAuthory.Stock_Screen + "," + UserAuthory.Stock_Update))]
        [Route("GetSelectedActivestock")]
        [HttpGet]
        public IActionResult GetSelectedActivestock(int ID)
        {
            ActivestockModel Data = unitOfWork.ActivestockRepository.Getbyid(ID);
            Data.Stock = unitOfWork.StockRepository.GetStockByGuid(Data.Stockid);
            Data.Department = unitOfWork.DepartmentRepository.GetDepartmentByGuid(Data.Departmentid);
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

        [Route("AddRange")]
        [Authorize(Roles = UserAuthory.Stock_Add)]
        [HttpPost]
        public IActionResult AddRange(List<ActivestockModel> list)
        {
            var username = (this.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name)?.Value;
            foreach (var item in list)
            {
                item.Stockid = item.Stock.ConcurrencyStamp;
                item.Departmentid = item.Department.ConcurrencyStamp;
                item.CreatedUser = username;
                item.IsActive = true;
                item.CreateTime = DateTime.Now;
                item.ConcurrencyStamp = Guid.NewGuid().ToString();
            }
            unitOfWork.ActivestockRepository.AddRange(list);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Update")]
        [Authorize(Roles = UserAuthory.Stock_Update)]
        [HttpPost]
        public IActionResult Update(ActivestockModel model)
        {
            var username = (this.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name)?.Value;
            model.Stockid = model.Stock.ConcurrencyStamp;
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.ActivestockRepository.update(unitOfWork.ActivestockRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Delete")]
        [Authorize(Roles = UserAuthory.Stock_Delete)]
        [HttpDelete]
        public IActionResult Delete(ActivestockModel model)
        {
            var username = (this.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name)?.Value;
            model.DeleteUser = username;
            model.DeleteTime = DateTime.Now;
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
