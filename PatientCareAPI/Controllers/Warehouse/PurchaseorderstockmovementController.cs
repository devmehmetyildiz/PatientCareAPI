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
    public class PurchaseorderstockmovementController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<PurchaseorderstockmovementController> _logger;
        private readonly ApplicationDBContext _context;
        Utilities Utilities;
        UnitOfWork unitOfWork;
        public PurchaseorderstockmovementController(IConfiguration configuration, ILogger<PurchaseorderstockmovementController> logger, ApplicationDBContext context)
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

        private List<PurchaseorderstocksmovementModel> FetchList()
        {
            var List = unitOfWork.PurchaseorderstocksmovementRepository.GetRecords<PurchaseorderstocksmovementModel>(u => u.IsActive);
            foreach (var item in List)
            {
                item.Stock = unitOfWork.PurchaseorderstocksRepository.GetRecord<PurchaseorderstocksModel>(u => u.ConcurrencyStamp == item.StockID);
                item.Stock.Stockdefine = unitOfWork.StockdefineRepository.GetRecord<StockdefineModel>(u => u.ConcurrencyStamp == item.Stock.StockdefineID);
                item.Stock.Department = unitOfWork.DepartmentRepository.GetRecord<DepartmentModel>(u => u.ConcurrencyStamp == item.Stock.Departmentid);
                item.Stock.Stockdefine.Unit = unitOfWork.UnitRepository.GetRecord<UnitModel>(u => u.ConcurrencyStamp == item.Stock.Stockdefine.Unitid);
            }
            return List;
        }

        [Route("GetAll")]
        [AuthorizeMultiplePolicy(UserAuthory.Purchaseorderstockmovement_Screen)]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(FetchList());
        }

        [Route("GetSelected")]
        [AuthorizeMultiplePolicy((UserAuthory.Purchaseorderstockmovement_Getselected))]
        [HttpGet]
        public IActionResult GetSelected(string guid)
        {
            PurchaseorderstocksmovementModel Data = unitOfWork.PurchaseorderstocksmovementRepository.GetRecord<PurchaseorderstocksmovementModel>(u => u.ConcurrencyStamp == guid);
            Data.Stock = unitOfWork.PurchaseorderstocksRepository.GetRecord<PurchaseorderstocksModel>(u => u.ConcurrencyStamp == Data.StockID);
            Data.Stock.Stockdefine = unitOfWork.StockdefineRepository.GetRecord<StockdefineModel>(u => u.ConcurrencyStamp == Data.Stock.StockdefineID);
            Data.Stock.Department = unitOfWork.DepartmentRepository.GetRecord<DepartmentModel>(u => u.ConcurrencyStamp == Data.Stock.Departmentid);
            Data.Stock.Stockdefine.Unit = unitOfWork.UnitRepository.GetRecord<UnitModel>(u => u.ConcurrencyStamp == Data.Stock.Stockdefine.Unitid);
            return Ok(Data);
        }

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Purchaseorderstockmovement_Add)]
        [HttpPost]
        public IActionResult Add(PurchaseorderstocksmovementModel model)
        {
            var username = GetSessionUser();
            string guid = Guid.NewGuid().ToString();
            model.CreatedUser = username;
            model.CreateTime = DateTime.Now;
            model.IsActive = true;
            model.ConcurrencyStamp = guid;

            double amount = 0;
            var movements = unitOfWork.PurchaseorderstocksmovementRepository.GetRecords<PurchaseorderstocksmovementModel>(u => u.StockID == model.StockID&& u.IsActive);
            foreach (var movement in movements)
            {
                amount += (movement.Amount * movement.Movementtype);
            }
            model.Prevvalue = amount;
            model.Newvalue = amount + (model.Amount * model.Movementtype);
            model.Movementdate = DateTime.Now;
            unitOfWork.PurchaseorderstocksmovementRepository.Add(model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy(UserAuthory.Purchaseorderstockmovement_Edit)]
        [HttpPost]
        public IActionResult Update(PurchaseorderstocksmovementModel model)
        {
            var username = GetSessionUser();
            PurchaseorderstocksmovementModel oldmodel = unitOfWork.PurchaseorderstocksmovementRepository.GetRecord<PurchaseorderstocksmovementModel>(u => u.ConcurrencyStamp == model.ConcurrencyStamp);
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.PurchaseorderstocksmovementRepository.update(oldmodel, model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Delete")]
        [AuthorizeMultiplePolicy(UserAuthory.Purchaseorderstockmovement_Delete)]
        [HttpPost]
        public IActionResult Delete(string guid)
        {
            var username = GetSessionUser();
            PurchaseorderstocksmovementModel oldmodel = unitOfWork.PurchaseorderstocksmovementRepository.GetRecord<PurchaseorderstocksmovementModel>(u => u.ConcurrencyStamp == guid);
            oldmodel.DeleteUser = username;
            oldmodel.DeleteTime = DateTime.Now;
            oldmodel.IsActive = false;
            unitOfWork.PurchaseorderstocksmovementRepository.update(unitOfWork.PurchaseorderstocksmovementRepository.Getbyid(oldmodel.Id), oldmodel);
            unitOfWork.Complate();
            return Ok(FetchList());
        }
    }
}
