using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PatientCareAPI.DataAccess;
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
    public class WarehouseController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<WarehouseController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;
        Utilities Utilities;
        // TODO: update ve delete durumları işlenecek
        public WarehouseController(IConfiguration configuration, ILogger<WarehouseController> logger, ApplicationDBContext context)
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

        private List<WarehouseModel> FetchList()
        {
            var List = unitOfWork.WarehouseRepository.GetRecords<WarehouseModel>(u => u.IsActive);
            foreach (var item in List)
            {
                item.Items = unitOfWork.StockRepository.GetRecords<StockModel>(u => u.IsActive && u.WarehouseID == item.ConcurrencyStamp);
                foreach (var stockitem in item.Items)
                {
                    double amount = 0;
                    var movements = unitOfWork.StockmovementRepository.GetRecords<StockmovementModel>(u => u.StockID == stockitem.ConcurrencyStamp);
                    foreach (var movement in movements)
                    {
                        amount += (movement.Amount * movement.Movementtype);
                    }
                    stockitem.Amount = amount;
                    stockitem.Stockdefine = unitOfWork.StockdefineRepository.GetSingleRecord<StockdefineModel>(u => u.ConcurrencyStamp == stockitem.StockdefineID);
                    stockitem.Department = unitOfWork.DepartmentRepository.GetSingleRecord<DepartmentModel>(u => u.ConcurrencyStamp == stockitem.Departmentid);
                    stockitem.Stockdefine.Unit = unitOfWork.UnitRepository.GetSingleRecord<UnitModel>(u => u.ConcurrencyStamp == stockitem.Stockdefine.Unitid);
                }
            }
            return List;
        }

        [HttpGet]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Screen)]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(FetchList());
        }

        [Route("GetSelected")]
        [AuthorizeMultiplePolicy((UserAuthory.Patients_Screen + "," + UserAuthory.Patients_Update))]
        [HttpGet]
        public IActionResult GetSelected(string guid)
        {
            var Data = unitOfWork.WarehouseRepository.GetSingleRecord<WarehouseModel>(u => u.IsActive && u.ConcurrencyStamp == guid);
            Data.Items = unitOfWork.StockRepository.GetRecords<StockModel>(u => u.IsActive && u.WarehouseID == Data.ConcurrencyStamp);
            foreach (var stockitem in Data.Items)
            {
                double amount = 0;
                var movements = unitOfWork.StockmovementRepository.GetRecords<StockmovementModel>(u => u.StockID == stockitem.ConcurrencyStamp);
                foreach (var movement in movements)
                {
                    amount += (movement.Amount * movement.Movementtype);
                }
                stockitem.Stockdefine = unitOfWork.StockdefineRepository.GetSingleRecord<StockdefineModel>(u => u.ConcurrencyStamp == stockitem.StockdefineID);
                stockitem.Department = unitOfWork.DepartmentRepository.GetSingleRecord<DepartmentModel>(u => u.ConcurrencyStamp == stockitem.Departmentid);
                stockitem.Stockdefine.Unit = unitOfWork.UnitRepository.GetSingleRecord<UnitModel>(u => u.ConcurrencyStamp == stockitem.Stockdefine.Unitid);
            }
            return Ok(Data);
        }

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Add)]
        [HttpPost]
        public IActionResult Add(WarehouseModel model)
        {
            var username = GetSessionUser();
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.WarehouseRepository.Add(model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Update)]
        [HttpPost]
        public IActionResult Update(WarehouseModel model)
        {
            var username = GetSessionUser();
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.WarehouseRepository.update(unitOfWork.WarehouseRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Delete")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Delete)]
        [HttpPost]
        public IActionResult Delete(WarehouseModel model)
        {
            var username = GetSessionUser();
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.WarehouseRepository.update(unitOfWork.WarehouseRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("DeleteFromDB")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(WarehouseModel model)
        {
            unitOfWork.WarehouseRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
