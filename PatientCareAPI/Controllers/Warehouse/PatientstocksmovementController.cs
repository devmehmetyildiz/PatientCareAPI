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
    public class PatientstocksmovementController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<PatientstocksmovementController> _logger;
        private readonly ApplicationDBContext _context;
        Utilities Utilities;
        UnitOfWork unitOfWork;
        public PatientstocksmovementController(IConfiguration configuration, ILogger<PatientstocksmovementController> logger, ApplicationDBContext context)
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
                item.Stock.Stockdefine = unitOfWork.StockdefineRepository.GetSingleRecord<StockdefineModel>(u => u.ConcurrencyStamp == item.Stock.StockdefineID);
                item.Stock.Department = unitOfWork.DepartmentRepository.GetSingleRecord<DepartmentModel>(u => u.ConcurrencyStamp == item.Stock.Departmentid);
                item.Stock.Stockdefine.Unit = unitOfWork.UnitRepository.GetSingleRecord<UnitModel>(u => u.ConcurrencyStamp == item.Stock.Stockdefine.Unitid);
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
            Data.Stock.Stockdefine = unitOfWork.StockdefineRepository.GetSingleRecord<StockdefineModel>(u => u.ConcurrencyStamp == Data.Stock.StockdefineID);
            Data.Stock.Department = unitOfWork.DepartmentRepository.GetSingleRecord<DepartmentModel>(u => u.ConcurrencyStamp == Data.Stock.Departmentid);
            Data.Stock.Stockdefine.Unit = unitOfWork.UnitRepository.GetSingleRecord<UnitModel>(u => u.ConcurrencyStamp == Data.Stock.Stockdefine.Unitid);
            return Ok(Data);
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
    }
}
