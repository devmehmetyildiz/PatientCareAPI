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
using System.Security.Claims;
using System.Threading.Tasks;

namespace PatientCareAPI.Controllers.Application
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseorderController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<StockController> _logger;
        private readonly ApplicationDBContext _context;
        Utilities Utilities;
        UnitOfWork unitOfWork;
        public PurchaseorderController(IConfiguration configuration, ILogger<StockController> logger, ApplicationDBContext context)
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
                List<string> Stockguids = unitOfWork.PurchaseorderToStockRepository.GetRecords<PurchaseorderToStockModel>(u => u.PurchaseID == purchaseorder.ConcurrencyStamp).ToList().Select(u => u.StockID).ToList();
                List<StockModel> Stocks = unitOfWork.StockRepository.GetStocksbyGuids(Stockguids);
                purchaseorder.Case = unitOfWork.CaseRepository.GetSingleRecord<CaseModel>(u => u.ConcurrencyStamp == purchaseorder.CaseID);
                foreach (var item in Stocks)
                {
                    item.Stockdefine = unitOfWork.StockdefineRepository.GetSingleRecord<StockdefineModel>(u => u.ConcurrencyStamp == item.Stockid);
                    item.Department = unitOfWork.DepartmentRepository.GetSingleRecord<DepartmentModel>(u => u.ConcurrencyStamp == item.Departmentid);
                    item.Stockdefine.Unit = unitOfWork.UnitRepository.GetSingleRecord<UnitModel>(u => u.ConcurrencyStamp == item.Stockdefine.Unitid);
                }
                purchaseorder.Stocks = Stocks;
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
            List<string> Stockguids = unitOfWork.PurchaseorderToStockRepository.GetRecords<PurchaseorderToStockModel>(u => u.PurchaseID == guid).ToList().Select(u => u.StockID).ToList();
            List<StockModel> Stocks = unitOfWork.StockRepository.GetStocksbyGuids(Stockguids);
            Data.Case = unitOfWork.CaseRepository.GetSingleRecord<CaseModel>(u => u.ConcurrencyStamp == Data.CaseID);
            foreach (var item in Stocks)
            {
            item.Stockdefine = unitOfWork.StockdefineRepository.GetSingleRecord<StockdefineModel>(u => u.ConcurrencyStamp == item.Stockid);
            item.Department = unitOfWork.DepartmentRepository.GetSingleRecord<DepartmentModel>(u => u.ConcurrencyStamp == item.Departmentid);
            item.Stockdefine.Unit = unitOfWork.UnitRepository.GetSingleRecord<UnitModel>(u => u.ConcurrencyStamp == item.Stockdefine.Unitid);
            }
            Data.Stocks = Stocks;
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
                unitOfWork.PurchaseorderToStockRepository.Add(new PurchaseorderToStockModel { PurchaseID = model.ConcurrencyStamp, StockID = stockguid });
                stock.CreatedUser = username;
                stock.CreateTime = DateTime.Now;
                stock.IsActive = true;
                stock.ConcurrencyStamp = stockguid;
                unitOfWork.StockRepository.Add(stock);
                unitOfWork.StockmovementRepository.Add(new StockmovementModel
                {
                    Activestockid = guid,
                    Amount = stock.Amount,
                    Movementdate = DateTime.Now,
                    Movementtype = ((int)Constants.Movementtypes.Create),
                    Prevvalue = 0,
                    Newvalue = stock.Amount,
                    UserID = unitOfWork.UsersRepository.FindUserByName(username).ConcurrencyStamp,
                });
            }
            unitOfWork.PurchaseorderRepository.Add(model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

       

        //[Route("Update")]
        //[AuthorizeMultiplePolicy(UserAuthory.Stock_Update)]
        //[HttpPost]
        //public IActionResult Update(PurchaseorderModel model)
        //{
        //    var username = GetSessionUser();
        //    StockModel oldmodel = unitOfWork.StockRepository.GetSingleRecord<StockModel>(u => u.ConcurrencyStamp == model.ConcurrencyStamp);
        //    unitOfWork.StockmovementRepository.Add(new StockmovementModel
        //    {
        //        Activestockid = model.ConcurrencyStamp,
        //        Amount = model.Amount - oldmodel.Amount,
        //        Movementdate = DateTime.Now,
        //        Movementtype = Getmovementtype(oldmodel.Amount, model.Amount),
        //        Prevvalue = oldmodel.Amount,
        //        Newvalue = model.Amount,
        //        UserID = unitOfWork.UsersRepository.FindUserByName(username).ConcurrencyStamp
        //    });
        //    model.Stockid = model.Stockdefine.ConcurrencyStamp;
        //    model.UpdatedUser = username;
        //    model.UpdateTime = DateTime.Now;
        //    unitOfWork.StockRepository.update(unitOfWork.StockRepository.Getbyid(model.Id), model);
        //    unitOfWork.Complate();
        //    return Ok(FetchList());
        //}

        //[Route("Delete")]
        //[AuthorizeMultiplePolicy(UserAuthory.Stock_Delete)]
        //[HttpDelete]
        //public IActionResult Delete(PurchaseorderModel model)
        //{
        //    var username = (this.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name)?.Value;
        //    StockModel oldmodel = unitOfWork.StockRepository.Getbyid(model.Id);
        //    unitOfWork.StockmovementRepository.Add(new StockmovementModel
        //    {
        //        Activestockid = model.ConcurrencyStamp,
        //        Amount = model.Amount,
        //        Movementdate = DateTime.Now,
        //        Movementtype = (int)Constants.Movementtypes.Delete,
        //        Prevvalue = model.Amount,
        //        Newvalue = model.Amount,
        //        UserID = unitOfWork.UsersRepository.FindUserByName(username).ConcurrencyStamp
        //    });
        //    model.DeleteUser = username;
        //    model.DeleteTime = DateTime.Now;
        //    unitOfWork.StockRepository.update(unitOfWork.StockRepository.Getbyid(model.Id), model);
        //    unitOfWork.Complate();
        //    return Ok(FetchList());
        //}

        [Route("DeleteFromDB")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(PurchaseorderModel model)
        {
            unitOfWork.PurchaseorderRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }


        private int Getmovementtype(double oldamount, double newamount)
        {
            var change = newamount - oldamount;
            if (change > 0)
                return (int)Constants.Movementtypes.Add;
            if (change == 0)
                return (int)Constants.Movementtypes.Update;
            if (change < 0)
                return (int)Constants.Movementtypes.Reduce;
            return 0;
        }

    }

   
   
}
