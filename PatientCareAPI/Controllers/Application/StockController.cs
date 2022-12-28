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

namespace PatientCareAPI.Controllers.Application
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
                 item.Stockdefine = unitOfWork.StockdefineRepository.GetSingleRecord<StockdefineModel>(u => u.ConcurrencyStamp == item.Stockid);
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
            Data.Stockdefine = unitOfWork.StockdefineRepository.GetSingleRecord<StockdefineModel>(u => u.ConcurrencyStamp == Data.Stockid);
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
            unitOfWork.DeactivestockRepository.Add(new DeactivestockModel
            {
                Activestockid = guid,
                Amount = model.Amount,
                Createduser = username,
                Createtime = DateTime.Now
            });
            model.IsActive = false;
            model.Deactivetime = DateTime.Now;
            model.DeleteTime = DateTime.Now;
            model.DeleteUser = username;
            model.Isdeactive = true;
            unitOfWork.StockRepository.update(unitOfWork.StockRepository.Getbyid(model.Id), model);
            unitOfWork.StockmovementRepository.Add(new StockmovementModel
            {
                Activestockid = guid,
                Amount= model.Amount,
                Movementdate = DateTime.Now,
                Movementtype = ((int)Constants.Movementtypes.Kill),
                Prevvalue = model.Amount,
                Newvalue = 0,
                UserID = unitOfWork.UsersRepository.FindUserByName(username).ConcurrencyStamp
            });
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
            model.Stockid = model.Stockdefine.ConcurrencyStamp;
            model.Departmentid = model.Department.ConcurrencyStamp;
            unitOfWork.StockRepository.Add(model);
            unitOfWork.StockmovementRepository.Add(new StockmovementModel
            {
                Activestockid = guid,
                Amount = model.Amount,
                Movementdate = DateTime.Now,
                Movementtype = ((int)Constants.Movementtypes.Create),
                Prevvalue = 0,
                Newvalue = model.Amount,
                UserID = unitOfWork.UsersRepository.FindUserByName(username).ConcurrencyStamp,
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
                    Activestockid = guid,
                    Amount = item.Amount,
                    Movementdate = DateTime.Now,
                    Movementtype = ((int)Constants.Movementtypes.Create),
                    Prevvalue = 0,
                    Newvalue = item.Amount,
                    UserID = unitOfWork.UsersRepository.FindUserByName(username).ConcurrencyStamp
                });
                item.Stockid = item.Stockdefine.ConcurrencyStamp;
                item.Departmentid = item.Department.ConcurrencyStamp;
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
            unitOfWork.StockmovementRepository.Add(new StockmovementModel
            {
                Activestockid = model.ConcurrencyStamp,
                Amount = model.Amount-oldmodel.Amount,
                Movementdate = DateTime.Now,
                Movementtype = Getmovementtype(oldmodel.Amount,model.Amount),
                Prevvalue = oldmodel.Amount,
                Newvalue = model.Amount,
                UserID = unitOfWork.UsersRepository.FindUserByName(username).ConcurrencyStamp
            });
            model.Stockid = model.Stockdefine.ConcurrencyStamp;
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
            unitOfWork.StockmovementRepository.Add(new StockmovementModel
            {
                Activestockid = model.ConcurrencyStamp,
                Amount = model.Amount,
                Movementdate = DateTime.Now,
                Movementtype = (int)Constants.Movementtypes.Delete,
                Prevvalue = model.Amount,
                Newvalue = model.Amount,
                UserID = unitOfWork.UsersRepository.FindUserByName(username).ConcurrencyStamp
            });
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


        private int Getmovementtype(double oldamount,double newamount)
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
