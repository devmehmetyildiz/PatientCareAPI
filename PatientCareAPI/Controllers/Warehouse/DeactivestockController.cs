using Microsoft.AspNetCore.Authorization;
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
    public class DeactivestockController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<DeactivestockController> _logger;
        private readonly ApplicationDBContext _context;
        Utilities Utilities;
        UnitOfWork unitOfWork;
        // TODO: Deaktive stok geri dönderme düzenlenecek
        public DeactivestockController(IConfiguration configuration, ILogger<DeactivestockController> logger, ApplicationDBContext context)
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

        private List<DeactivestockModel> FetchList()
        {
            var List = unitOfWork.DeactivestockRepository.GetAll();
            foreach (var item in List)
            {
                item.Stock = unitOfWork.UnitRepository.GetSingleRecord<StockModel>(u => u.ConcurrencyStamp == item.StockID);
                item.Stock.Stockdefine = unitOfWork.StockdefineRepository.GetSingleRecord<StockdefineModel>(u => u.ConcurrencyStamp == item.Stock.StockdefineID);
                item.Department = unitOfWork.DepartmentRepository.GetSingleRecord<DepartmentModel>(u => u.ConcurrencyStamp == item.DepartmentID);
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

       
    }
}
