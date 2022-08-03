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

        [Route("Deactivestock")]
        [Authorize(Roles = UserAuthory.Stock_Screen)]
        [HttpPost]
        public IActionResult Deactivestock(string guid)
        {
            var username = (this.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name)?.Value;
            ActivestockModel model = unitOfWork.ActivestockRepository.GetStockByGuid(guid);
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
            unitOfWork.ActivestockRepository.update(unitOfWork.ActivestockRepository.Getbyid(model.Id), model);
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
        [Authorize(Roles = UserAuthory.Stock_Add)]
        [HttpPost]
        public IActionResult Add(ActivestockModel model)
        {
            var username = (this.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name)?.Value;
            model.CreatedUser = username;
            model.CreateTime = DateTime.Now;
            model.IsActive = true;
            model.Stockid = model.Stock.ConcurrencyStamp;
            unitOfWork.ActivestockRepository.Add(model);
            unitOfWork.StockmovementRepository.Add(new StockmovementModel
            {
                Activestockid = model.Stock.ConcurrencyStamp,
                Amount = model.Amount,
                Movementdate = DateTime.Now,
                Movementtype = ((int)Constants.Movementtypes.Create),
                Prevvalue = 0,
                Newvalue = model.Amount,
                UserID = unitOfWork.UsersRepository.FindUserByName(username).ConcurrencyStamp
            });
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
                unitOfWork.StockmovementRepository.Add(new StockmovementModel
                {
                    Activestockid = item.Stock.ConcurrencyStamp,
                    Amount = item.Amount,
                    Movementdate = DateTime.Now,
                    Movementtype = ((int)Constants.Movementtypes.Create),
                    Prevvalue = 0,
                    Newvalue = item.Amount,
                    UserID = unitOfWork.UsersRepository.FindUserByName(username).ConcurrencyStamp
                });
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
            ActivestockModel oldmodel = unitOfWork.ActivestockRepository.Getbyid(model.Id);
            unitOfWork.StockmovementRepository.Add(new StockmovementModel
            {
                Activestockid = model.Stock.ConcurrencyStamp,
                Amount = model.Amount,
                Movementdate = DateTime.Now,
                Movementtype = Getmovementtype(oldmodel.Amount,model.Amount),
                Prevvalue = oldmodel.Amount,
                Newvalue = model.Amount,
                UserID = unitOfWork.UsersRepository.FindUserByName(username).ConcurrencyStamp
            });
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
            ActivestockModel oldmodel = unitOfWork.ActivestockRepository.Getbyid(model.Id);
            unitOfWork.StockmovementRepository.Add(new StockmovementModel
            {
                Activestockid = model.Stock.ConcurrencyStamp,
                Amount = model.Amount,
                Movementdate = DateTime.Now,
                Movementtype = (int)Constants.Movementtypes.Delete,
                Prevvalue = model.Amount,
                Newvalue = model.Amount,
                UserID = unitOfWork.UsersRepository.FindUserByName(username).ConcurrencyStamp
            });
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
