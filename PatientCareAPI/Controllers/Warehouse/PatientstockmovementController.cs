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
    public class PatientstockmovementController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<PatientstockmovementController> _logger;
        private readonly ApplicationDBContext _context;
        Utilities Utilities;
        UnitOfWork unitOfWork;
        public PatientstockmovementController(IConfiguration configuration, ILogger<PatientstockmovementController> logger, ApplicationDBContext context)
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

        private List<PatientstocksmovementModel> FetchList()
        {
            var List = unitOfWork.PatientstocksmovementRepository.GetRecords<PatientstocksmovementModel>(u => u.IsActive);
            foreach (var item in List)
            {
                item.Stock = unitOfWork.PatientstocksRepository.GetSingleRecord<PatientstocksModel>(u => u.ConcurrencyStamp == item.StockID);
                if (item.Stock != null)
                {
                    item.Stock.Department = unitOfWork.DepartmentRepository.GetSingleRecord<DepartmentModel>(u => u.ConcurrencyStamp == item.Stock.Departmentid);
                    item.Stock.Stockdefine = unitOfWork.StockdefineRepository.GetSingleRecord<StockdefineModel>(u => u.ConcurrencyStamp == item.Stock.StockdefineID);
                    if (item.Stock.Stockdefine != null)
                    {
                        item.Stock.Stockdefine.Unit = unitOfWork.UnitRepository.GetSingleRecord<UnitModel>(u => u.ConcurrencyStamp == item.Stock.Stockdefine.Unitid);
                    }
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
        public IActionResult GetSelected(string guid)
        {
            PatientstocksmovementModel Data = unitOfWork.PatientstocksmovementRepository.GetSingleRecord<PatientstocksmovementModel>(u => u.ConcurrencyStamp == guid);
            Data.Stock = unitOfWork.PatientstocksRepository.GetSingleRecord<PatientstocksModel>(u => u.ConcurrencyStamp == Data.StockID);
            if (Data.Stock != null)
            {
                Data.Stock.Department = unitOfWork.DepartmentRepository.GetSingleRecord<DepartmentModel>(u => u.ConcurrencyStamp == Data.Stock.Departmentid);
                Data.Stock.Stockdefine = unitOfWork.StockdefineRepository.GetSingleRecord<StockdefineModel>(u => u.ConcurrencyStamp == Data.Stock.StockdefineID);
                if (Data.Stock.Stockdefine != null)
                {
                    Data.Stock.Stockdefine.Unit = unitOfWork.UnitRepository.GetSingleRecord<UnitModel>(u => u.ConcurrencyStamp == Data.Stock.Stockdefine.Unitid);
                }
            }
            return Ok(Data);
        }

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Stock_Update)]
        [HttpPost]
        public IActionResult Add(PatientstocksmovementModel model)
        {
            var username = GetSessionUser();
            string guid = Guid.NewGuid().ToString();
            model.CreatedUser = username;
            model.CreateTime = DateTime.Now;
            model.IsActive = true;
            model.ConcurrencyStamp = guid;

            double amount = 0;
            var movements = unitOfWork.PatientstocksmovementRepository.GetRecords<PatientstocksmovementModel>(u => u.StockID == model.StockID && u.IsActive);
            foreach (var movement in movements)
            {
                amount += (movement.Amount * movement.Movementtype);
            }
            model.Prevvalue = amount;
            model.Newvalue = amount + (model.Amount * model.Movementtype);
            model.Movementdate = DateTime.Now;
            unitOfWork.PatientstocksmovementRepository.Add(model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy(UserAuthory.Stock_Update)]
        [HttpPost]
        public IActionResult Update(PatientstocksmovementModel model)
        {
            var username = GetSessionUser();
            PatientstocksmovementModel oldmodel = unitOfWork.PatientstocksmovementRepository.GetSingleRecord<PatientstocksmovementModel>(u => u.ConcurrencyStamp == model.ConcurrencyStamp);
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.PatientstocksmovementRepository.update(oldmodel, model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Delete")]
        [AuthorizeMultiplePolicy(UserAuthory.Stock_Update)]
        [HttpPost]
        public IActionResult Delete(string guid)
        {
            var username = GetSessionUser();
            PatientstocksmovementModel oldmodel = unitOfWork.PatientstocksmovementRepository.GetSingleRecord<PatientstocksmovementModel>(u => u.ConcurrencyStamp == guid);
            oldmodel.DeleteUser = username;
            oldmodel.DeleteTime = DateTime.Now;
            oldmodel.IsActive = false;
            unitOfWork.PatientstocksmovementRepository.update(unitOfWork.PatientstocksmovementRepository.Getbyid(oldmodel.Id), oldmodel);
            unitOfWork.Complate();
            return Ok(FetchList());
        }
    }
}
