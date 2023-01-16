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
    public class PatientstocksController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<PatientstocksController> _logger;
        private readonly ApplicationDBContext _context;
        Utilities Utilities;
        UnitOfWork unitOfWork;
        public PatientstocksController(IConfiguration configuration, ILogger<PatientstocksController> logger, ApplicationDBContext context)
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

        private List<PatientstocksModel> FetchList()
        {
            var List = unitOfWork.PatientstocksRepository.GetRecords<PatientstocksModel>(u => u.IsActive);
            foreach (var item in List)
            {
                double amount = 0;
                var movements = unitOfWork.PatientstocksmovementRepository.GetRecords<PatientstocksmovementModel>(u => u.StockID == item.ConcurrencyStamp);
                foreach (var movement in movements)
                {
                    amount += (movement.Amount * movement.Movementtype);
                }
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
        public IActionResult GetSelected(string guid)
        {
            PatientstocksModel Data = unitOfWork.PatientstocksRepository.GetSingleRecord<PatientstocksModel>(u => u.ConcurrencyStamp == guid);
            double amount = 0;
            var movements = unitOfWork.PatientstocksmovementRepository.GetRecords<PatientstocksmovementModel>(u => u.StockID == Data.ConcurrencyStamp);
            foreach (var movement in movements)
            {
                amount += (movement.Amount * movement.Movementtype);
            }
            Data.Stockdefine = unitOfWork.StockdefineRepository.GetSingleRecord<StockdefineModel>(u => u.ConcurrencyStamp == Data.StockdefineID);
            Data.Department = unitOfWork.DepartmentRepository.GetSingleRecord<DepartmentModel>(u => u.ConcurrencyStamp == Data.Departmentid);
            Data.Stockdefine.Unit = unitOfWork.UnitRepository.GetSingleRecord<UnitModel>(u => u.ConcurrencyStamp == Data.Stockdefine.Unitid);
            return Ok(Data);
        }

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Stock_Add)]
        [HttpPost]
        public IActionResult Add(PatientstocksModel model)
        {
            string stockguid = Guid.NewGuid().ToString();
            model.CreatedUser = GetSessionUser();
            model.CreateTime = DateTime.Now;
            model.IsActive = true;
            model.ConcurrencyStamp = stockguid;
            unitOfWork.PatientstocksRepository.Add(model);
            unitOfWork.PatientstocksmovementRepository.Add(new PatientstocksmovementModel
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
        public IActionResult Update(PatientstocksModel model)
        {
            var username = GetSessionUser();
            PatientstocksModel oldmodel = unitOfWork.PatientstocksRepository.GetSingleRecord<PatientstocksModel>(u => u.ConcurrencyStamp == model.ConcurrencyStamp);
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.PatientstocksRepository.update(oldmodel, model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("DeleteFromDB")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(PatientstocksModel model)
        {
            unitOfWork.PatientstocksRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
