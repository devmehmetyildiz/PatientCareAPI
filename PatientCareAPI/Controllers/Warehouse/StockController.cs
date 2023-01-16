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
using PatientCareAPI.Models.Settings;
using PatientCareAPI.Models.Warehouse;

namespace PatientCareAPI.Controllers.Warehouse
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<StockController> _logger;
        private readonly ApplicationDBContext _context;
        Utilities Utilities;
        UnitOfWork unitOfWork;
        // TODO: delete durumları düzenlenecek
        // TODO: Add amount seçeneği eklenecek???
        public StockController(IConfiguration configuration, ILogger<StockController> logger, ApplicationDBContext context)
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

        private List<StockModel> FetchList()
        {
            var List = unitOfWork.StockRepository.GetRecords<StockModel>(u => u.IsActive && !u.Isdeactive);
            foreach (var item in List)
            {
                double amount = 0;
                var movements = unitOfWork.StockmovementRepository.GetRecords<StockmovementModel>(u => u.StockID == item.ConcurrencyStamp);
                foreach (var movement in movements)
                {
                    amount += (movement.Amount * movement.Movementtype);
                }
                item.Amount = amount;
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
            StockModel Data = unitOfWork.StockRepository.GetSingleRecord<StockModel>(u => u.ConcurrencyStamp == guid);
            double amount = 0;
            var movements = unitOfWork.StockmovementRepository.GetRecords<StockmovementModel>(u => u.StockID == Data.ConcurrencyStamp);
            foreach (var movement in movements)
            {
                amount += (movement.Amount * movement.Movementtype);
            }
            Data.Amount = amount;
            Data.Stockdefine = unitOfWork.StockdefineRepository.GetSingleRecord<StockdefineModel>(u => u.ConcurrencyStamp == Data.StockdefineID);
            Data.Department = unitOfWork.DepartmentRepository.GetSingleRecord<DepartmentModel>(u => u.ConcurrencyStamp == Data.Departmentid);
            Data.Stockdefine.Unit = unitOfWork.UnitRepository.GetSingleRecord<UnitModel>(u => u.ConcurrencyStamp == Data.Stockdefine.Unitid);
            return Ok(Data);
        }

        [Route("Deactivestocks")]
        [AuthorizeMultiplePolicy(UserAuthory.Stock_Screen)]
        [HttpPost]
        public IActionResult Deactivestock(string guid)
        {
            var username = GetSessionUser();
            StockModel model = unitOfWork.StockRepository.GetStockByGuid(guid);
            double amount = 0;
            var movements = unitOfWork.StockmovementRepository.GetRecords<StockmovementModel>(u => u.StockID == guid);
            foreach (var movement in movements)
            {
                amount += (movement.Amount * movement.Movementtype);
            }
            unitOfWork.DeactivestockRepository.Add(new DeactivestockModel
            {
                StockID = guid,
                DepartmentID = model.Departmentid,
                Amount = amount,
                Createduser = username,
                Createtime = DateTime.Now
            });
            model.IsActive = false;
            model.Deactivetime = DateTime.Now;
            model.DeleteTime = DateTime.Now;
            model.DeleteUser = username;
            model.Isdeactive = true;
            unitOfWork.StockRepository.update(unitOfWork.StockRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Stock_Add)]
        [HttpPost]
        public IActionResult Add(StockModel model)
        {
            string guid = Guid.NewGuid().ToString();
            var username = GetSessionUser();
            model.CreatedUser = username;
            model.CreateTime = DateTime.Now;
            model.IsActive = true;
            model.ConcurrencyStamp = guid;
            unitOfWork.StockRepository.Add(model);
            unitOfWork.StockmovementRepository.Add(new StockmovementModel
            {
                StockID = guid,
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

        [Route("AddRange")]
        [AuthorizeMultiplePolicy(UserAuthory.Stock_Add)]
        [HttpPost]
        public IActionResult AddRange(List<StockModel> list)
        {
            var username = GetSessionUser();
            foreach (var item in list)
            {
                string guid = Guid.NewGuid().ToString();
                unitOfWork.StockmovementRepository.Add(new StockmovementModel
                {
                    StockID = guid,
                    Amount = item.Amount,
                    Movementdate = DateTime.Now,
                    Movementtype = ((int)Constants.Movementtypes.income),
                    Prevvalue = 0,
                    Newvalue = item.Amount,
                    CreatedUser = GetSessionUser(),
                    CreateTime = DateTime.Now
                });
                item.CreatedUser = username;
                item.IsActive = true;
                item.CreateTime = DateTime.Now;
                item.ConcurrencyStamp = guid;
            }
            unitOfWork.StockRepository.AddRange(list);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy(UserAuthory.Stock_Update)]
        [HttpPost]
        public IActionResult Update(StockModel model)
        {
            var username = GetSessionUser();
            StockModel oldmodel = unitOfWork.StockRepository.GetSingleRecord<StockModel>(u => u.ConcurrencyStamp == model.ConcurrencyStamp);
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.StockRepository.update(unitOfWork.StockRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Delete")]
        [AuthorizeMultiplePolicy(UserAuthory.Stock_Delete)]
        [HttpDelete]
        public IActionResult Delete(StockModel model)
        {
            var username = (this.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name)?.Value;
            StockModel oldmodel = unitOfWork.StockRepository.Getbyid(model.Id);
            model.DeleteUser = username;
            model.DeleteTime = DateTime.Now;
            unitOfWork.StockRepository.update(unitOfWork.StockRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("DeleteFromDB")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(StockModel model)
        {
            unitOfWork.StockRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
