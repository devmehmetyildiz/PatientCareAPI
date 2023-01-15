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
    public class PurchaseorderstockController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<PurchaseorderstockController> _logger;
        private readonly ApplicationDBContext _context;
        Utilities Utilities;
        UnitOfWork unitOfWork;
        public PurchaseorderstockController(IConfiguration configuration, ILogger<PurchaseorderstockController> logger, ApplicationDBContext context)
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

        private List<PurchaseorderstocksModel> FetchList()
        {
            var List = unitOfWork.PurchaseorderstocksRepository.GetRecords<PurchaseorderstocksModel>(u => u.IsActive);
            foreach (var item in List)
            {
                item.Stockdefine = unitOfWork.StockdefineRepository.GetSingleRecord<StockdefineModel>(u => u.ConcurrencyStamp == item.StockdefineID);
                item.Department = unitOfWork.DepartmentRepository.GetSingleRecord<DepartmentModel>(u => u.ConcurrencyStamp == item.Departmentid);
                item.Stockdefine.Unit = unitOfWork.UnitRepository.GetSingleRecord<UnitModel>(u => u.ConcurrencyStamp == item.Stockdefine.Unitid);
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
            PurchaseorderstocksModel Data = unitOfWork.PurchaseorderstocksRepository.GetSingleRecord<PurchaseorderstocksModel>(u => u.ConcurrencyStamp == guid);
            Data.Stockdefine = unitOfWork.StockdefineRepository.GetSingleRecord<StockdefineModel>(u => u.ConcurrencyStamp == Data.StockdefineID);
            Data.Department = unitOfWork.DepartmentRepository.GetSingleRecord<DepartmentModel>(u => u.ConcurrencyStamp == Data.Departmentid);
            Data.Stockdefine.Unit = unitOfWork.UnitRepository.GetSingleRecord<UnitModel>(u => u.ConcurrencyStamp == Data.Stockdefine.Unitid);
            return Ok(Data);
        }

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Stock_Add)]
        [HttpPost]
        public IActionResult Add(PurchaseorderstocksModel model)
        {
            string stockguid = Guid.NewGuid().ToString();
            model.CreatedUser = GetSessionUser();
            model.CreateTime = DateTime.Now;
            model.IsActive = true;
            model.ConcurrencyStamp = stockguid;
            unitOfWork.PurchaseorderstocksRepository.Add(model);
            unitOfWork.PurchaseorderstocksmovementRepository.Add(new PurchaseorderstocksmovementModel
            {
                StockID = stockguid,
                Amount = model.Amount,
                Movementdate = DateTime.Now,
                Movementtype = ((int)Constants.Movementtypes.income),
                Prevvalue = 0,
                Newvalue = model.Amount,
                CreatedUser = GetSessionUser(),
                CreateTime = DateTime.Now
            });
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy(UserAuthory.Stock_Update)]
        [HttpPost]
        public IActionResult Update(PurchaseorderstocksModel model)
        {
            var username = GetSessionUser();
            PurchaseorderstocksModel oldmodel = unitOfWork.PurchaseorderstocksRepository.GetSingleRecord<PurchaseorderstocksModel>(u => u.ConcurrencyStamp == model.ConcurrencyStamp);
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.PurchaseorderstocksRepository.update(oldmodel, model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("DeleteFromDB")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(PurchaseorderstocksModel model)
        {
            unitOfWork.PurchaseorderstocksRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
