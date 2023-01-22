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
    public class PurchaseorderController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<PurchaseorderController> _logger;
        private readonly ApplicationDBContext _context;
        Utilities Utilities;
        UnitOfWork unitOfWork;
        // TODO: deactive durumlarında bağlı movement ve stoklarında deactive olması lazım

        public PurchaseorderController(IConfiguration configuration, ILogger<PurchaseorderController> logger, ApplicationDBContext context)
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

        private List<PurchaseorderModel> FetchList()
        {
            var List = unitOfWork.PurchaseorderRepository.GetRecords<PurchaseorderModel>(u => u.IsActive);
            foreach (var purchaseorder in List)
            {
                purchaseorder.Stocks = unitOfWork.PurchaseorderstocksRepository.GetRecords<PurchaseorderstocksModel>(u => u.PurchaseorderID == purchaseorder.ConcurrencyStamp && u.IsActive);
                purchaseorder.Case = unitOfWork.CaseRepository.GetSingleRecord<CaseModel>(u => u.ConcurrencyStamp == purchaseorder.CaseID && u.IsActive);
                foreach (var item in purchaseorder.Stocks)
                {
                    double amount = 0;
                    var movements = unitOfWork.PurchaseorderstocksmovementRepository.GetRecords<PurchaseorderstocksmovementModel>(u => u.StockID == item.ConcurrencyStamp && u.IsActive);
                    foreach (var movement in movements)
                    {
                        amount += (movement.Amount * movement.Movementtype);
                    }
                    item.Amount = amount;
                    item.Stockdefine = unitOfWork.StockdefineRepository.GetSingleRecord<StockdefineModel>(u => u.ConcurrencyStamp == item.StockdefineID);
                    item.Department = unitOfWork.DepartmentRepository.GetSingleRecord<DepartmentModel>(u => u.ConcurrencyStamp == item.Departmentid);
                    item.Stockdefine.Unit = unitOfWork.UnitRepository.GetSingleRecord<UnitModel>(u => u.ConcurrencyStamp == item.Stockdefine.Unitid);
                }
            }
            return List;
        }

        [Route("GetAll")]
        [AuthorizeMultiplePolicy(UserAuthory.Stock_Screen)]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(FetchList());
        }

        [Route("GetSelected")]
        [AuthorizeMultiplePolicy((UserAuthory.Stock_Screen + "," + UserAuthory.Stock_Update))]
        [HttpGet]
        public IActionResult GetSelectedActivestock(string guid)
        {
            PurchaseorderModel Data = unitOfWork.PurchaseorderRepository.GetSingleRecord<PurchaseorderModel>(u => u.ConcurrencyStamp == guid);
            Data.Stocks = unitOfWork.PurchaseorderstocksRepository.GetRecords<PurchaseorderstocksModel>(u => u.PurchaseorderID == Data.ConcurrencyStamp && u.IsActive);
            Data.Case = unitOfWork.CaseRepository.GetSingleRecord<CaseModel>(u => u.ConcurrencyStamp == Data.CaseID && u.IsActive);
            foreach (var item in Data.Stocks)
            {
                double amount = 0;
                var movements = unitOfWork.PurchaseorderstocksmovementRepository.GetRecords<PurchaseorderstocksmovementModel>(u => u.StockID == item.ConcurrencyStamp && u.IsActive);
                foreach (var movement in movements)
                {
                    amount += (movement.Amount * movement.Movementtype);
                }
                item.Amount = amount;
                item.Stockdefine = unitOfWork.StockdefineRepository.GetSingleRecord<StockdefineModel>(u => u.ConcurrencyStamp == item.StockdefineID);
                item.Department = unitOfWork.DepartmentRepository.GetSingleRecord<DepartmentModel>(u => u.ConcurrencyStamp == item.Departmentid);
                item.Stockdefine.Unit = unitOfWork.UnitRepository.GetSingleRecord<UnitModel>(u => u.ConcurrencyStamp == item.Stockdefine.Unitid);
            }
            return Ok(Data);
        }

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Stock_Add)]
        [HttpPost]
        public IActionResult Add(PurchaseorderModel model)
        {
            string guid = Guid.NewGuid().ToString();
            var username = GetSessionUser();
            model.CreatedUser = username;
            model.CreateTime = DateTime.Now;
            model.IsActive = true;
            model.ConcurrencyStamp = guid;
            foreach (var stock in model.Stocks)
            {
                string stockguid = Guid.NewGuid().ToString();
                stock.CreatedUser = username;
                stock.CreateTime = DateTime.Now;
                stock.IsActive = true;
                stock.ConcurrencyStamp = stockguid;
                stock.PurchaseorderID = guid;
                unitOfWork.PurchaseorderstocksRepository.Add(stock);
                unitOfWork.PurchaseorderstocksmovementRepository.Add(new PurchaseorderstocksmovementModel
                {
                    StockID = stockguid,
                    Amount = stock.Amount,
                    Movementdate = DateTime.Now,
                    Movementtype = ((int)Constants.Movementtypes.income),
                    Prevvalue = 0,
                    Newvalue = stock.Amount,
                    CreatedUser = GetSessionUser(),
                    CreateTime = DateTime.Now,
                    IsActive=true,
                     ConcurrencyStamp = Guid.NewGuid().ToString()
            });
            }
            unitOfWork.PurchaseorderRepository.Add(model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy(UserAuthory.Stock_Update)]
        [HttpPost]
        public IActionResult Update(PurchaseorderModel model)
        {
            var username = GetSessionUser();
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            foreach (var stock in model.Stocks)
            {
                if (stock.ConcurrencyStamp!=null && stock.ConcurrencyStamp!="")
                {
                    PurchaseorderstocksModel oldmodel = unitOfWork.PurchaseorderstocksRepository.GetSingleRecord<PurchaseorderstocksModel>(u => u.ConcurrencyStamp == stock.ConcurrencyStamp);
                    stock.UpdatedUser = username;
                    stock.UpdateTime = DateTime.Now;
                    unitOfWork.PurchaseorderstocksRepository.update(oldmodel, stock);
                }
                else
                {
                    string stockguid = Guid.NewGuid().ToString();
                    stock.CreatedUser = username;
                    stock.CreateTime = DateTime.Now;
                    stock.IsActive = true;
                    stock.ConcurrencyStamp = stockguid;
                    unitOfWork.PurchaseorderstocksRepository.Add(stock);
                    unitOfWork.PurchaseorderstocksmovementRepository.Add(new PurchaseorderstocksmovementModel
                    {
                        StockID = stockguid,
                        Amount = stock.Amount,
                        Movementdate = DateTime.Now,
                        Movementtype = ((int)Constants.Movementtypes.income),
                        Prevvalue = 0,
                        Newvalue = stock.Amount,
                        CreatedUser = GetSessionUser(),
                        CreateTime = DateTime.Now
                    });
                }
            }
            unitOfWork.PurchaseorderRepository.update(unitOfWork.PurchaseorderRepository.GetSingleRecord<PurchaseorderModel>(u => u.ConcurrencyStamp == model.ConcurrencyStamp), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Complete")]
        [AuthorizeMultiplePolicy(UserAuthory.Stock_Update)]
        [HttpPost]
        public IActionResult Complete(PurchaseorderModel model)
        {
            var username = GetSessionUser();
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            var caseID = unitOfWork.CaseRepository.GetRecords<CaseModel>(u => u.IsActive && u.CaseStatus == 1).FirstOrDefault()?.ConcurrencyStamp;
            if (caseID == null)
            {
                return new ObjectResult(new ResponseModel { Status = "Can't Complete", Massage = "Sisteme tanımlı Tamamlama durumu bulunamadı" }) { StatusCode = 403 };
            }
            model.CaseID = caseID;
            foreach (var stock in model.Stocks)
            {
                var foundedstock = unitOfWork.StockRepository.GetSingleRecord<StockModel>(u =>
                u.Skt == stock.Skt &&
                u.Barcodeno.Trim() == stock.Barcodeno.Trim() &&
                u.StockdefineID == stock.StockdefineID &&
                u.Departmentid == stock.Departmentid &&
                u.WarehouseID == model.WarehouseID);
                double amount = 0;
                var movements = unitOfWork.PurchaseorderstocksmovementRepository.GetRecords<PurchaseorderstocksmovementModel>(u => u.StockID == stock.ConcurrencyStamp && u.IsActive);
                foreach (var movement in movements)
                {
                    amount += (movement.Amount * movement.Movementtype);
                }
                if (foundedstock == null)
                {
                    string newStockguid = Guid.NewGuid().ToString();
                    unitOfWork.StockRepository.Add(new StockModel
                    {
                        CreatedUser = GetSessionUser(),
                        CreateTime = DateTime.Now,
                        IsActive=true,
                        Barcodeno = stock.Barcodeno,
                        ConcurrencyStamp = newStockguid,
                        Departmentid = stock.Departmentid,
                        Info = stock.Info,
                        Skt = stock.Skt,
                        StockdefineID = stock.StockdefineID,
                        WarehouseID = model.WarehouseID,
                    });
                    unitOfWork.StockmovementRepository.Add(new StockmovementModel
                    {
                        StockID = newStockguid,
                        Amount = amount,
                        Movementdate = DateTime.Now,
                        Movementtype = ((int)Constants.Movementtypes.income),
                        Prevvalue = 0,
                        Newvalue = amount,
                        CreatedUser = GetSessionUser(),
                        CreateTime = DateTime.Now,
                        IsActive = true,
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    });
                }
                else
                {
                    double previousamount = 0;
                    var oldmovements = unitOfWork.StockmovementRepository.GetRecords<StockmovementModel>(u => u.StockID == foundedstock.ConcurrencyStamp);
                    foreach (var movement in oldmovements)
                    {
                        previousamount += (movement.Amount * movement.Movementtype);
                    }
                    unitOfWork.StockmovementRepository.Add(new StockmovementModel
                    {
                        StockID = foundedstock.ConcurrencyStamp,
                        Amount = amount,
                        Movementdate = DateTime.Now,
                        Movementtype = ((int)Constants.Movementtypes.income),
                        Prevvalue = previousamount,
                        Newvalue = previousamount+amount,
                        CreatedUser = GetSessionUser(),
                        CreateTime = DateTime.Now,
                        IsActive = true,
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    });
                }
            }
            unitOfWork.PurchaseorderRepository.update(unitOfWork.PurchaseorderRepository.GetSingleRecord<PurchaseorderModel>(u => u.ConcurrencyStamp == model.ConcurrencyStamp), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }


        [Route("Deactive")]
        [AuthorizeMultiplePolicy(UserAuthory.Stock_Update)]
        [HttpPost]
        public IActionResult Deactive(PurchaseorderModel model)
        {
            var username = GetSessionUser();
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            var caseID = unitOfWork.CaseRepository.GetRecords<CaseModel>(u => u.IsActive && u.CaseStatus == -1).FirstOrDefault()?.ConcurrencyStamp;
            if (caseID == null)
            {
                return new ObjectResult(new ResponseModel { Status = "Can't Complete", Massage = "Sisteme tanımlı İptal etme durumu bulunamadı" }) { StatusCode = 403 };
            }
            model.CaseID = caseID;
            unitOfWork.PurchaseorderRepository.update(unitOfWork.PurchaseorderRepository.GetSingleRecord<PurchaseorderModel>(u => u.ConcurrencyStamp == model.ConcurrencyStamp), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("DeleteFromDB")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(PurchaseorderModel model)
        {
            unitOfWork.PurchaseorderRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
