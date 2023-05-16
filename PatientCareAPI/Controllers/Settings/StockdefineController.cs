using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PatientCareAPI.DataAccess;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using PatientCareAPI.Utils;
using PatientCareAPI.Models.Application;
using PatientCareAPI.Models.Warehouse;
using Faker;

namespace PatientCareAPI.Controllers.Settings
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StockdefineController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<StockdefineController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;
        Utilities Utilities;
        public StockdefineController(IConfiguration configuration, ILogger<StockdefineController> logger, ApplicationDBContext context)
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

        private List<StockdefineModel> FetchList()
        {
            var List = unitOfWork.StockdefineRepository.GetRecords<StockdefineModel>(u => u.IsActive);
            foreach (var item in List)
            {
                item.Unit = unitOfWork.UnitRepository.GetRecord<UnitModel>(u => u.ConcurrencyStamp == item.Unitid);
                item.Department = unitOfWork.DepartmentRepository.GetRecord<DepartmentModel>(u => u.ConcurrencyStamp == item.Departmentid);
            }
            return List;
        }

        [HttpGet]
        [AuthorizeMultiplePolicy(UserAuthory.Stockdefine_Screen)]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(FetchList());
        }

        [Route("GetSelected")]
        [AuthorizeMultiplePolicy((UserAuthory.Stockdefine_Getselected))]
        [HttpGet]
        public IActionResult GetSelectedStock(string guid)
        {
            StockdefineModel Data = unitOfWork.StockdefineRepository.GetRecord<StockdefineModel>(u => u.ConcurrencyStamp == guid);
            Data.Department = unitOfWork.DepartmentRepository.GetDepartmentByGuid(Data.Departmentid);
            Data.Unit = unitOfWork.UnitRepository.GetUnitByGuid(Data.Unitid);
            return Ok(Data);
        }

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Stockdefine_Add)]
        [HttpPost]
        public IActionResult Add(StockdefineModel model)
        {
            var username = GetSessionUser();
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            model.Departmentid = model.Department.ConcurrencyStamp;
            model.Unitid = model.Unit.ConcurrencyStamp;
            unitOfWork.StockdefineRepository.Add(model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy(UserAuthory.Stockdefine_Edit)]
        [HttpPost]
        public IActionResult Update(StockdefineModel model)
        {
            var username = GetSessionUser();
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            model.Departmentid = model.Department.ConcurrencyStamp;
            model.Unitid = model.Unit.ConcurrencyStamp;
            unitOfWork.StockdefineRepository.update(unitOfWork.StockdefineRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Delete")]
        [AuthorizeMultiplePolicy(UserAuthory.Stockdefine_Delete)]
        [HttpPost]
        public IActionResult Delete(StockdefineModel model)
        {
            var stocklist = unitOfWork.StockRepository.GetRecords<StockModel>(u=>u.IsActive&&u.StockdefineID==model.ConcurrencyStamp);
            if (stocklist.Count > 0)
            {
                return new ObjectResult(new ResponseModel { Status = "Can't Delete", Massage = model.Name + " ürününe bağlı aktif ürün var" }) { StatusCode = 403 };
            }
            var username = GetSessionUser();
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.StockdefineRepository.update(unitOfWork.StockdefineRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("DeleteFromDB")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(StockdefineModel model)
        {
            unitOfWork.StockRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Createfakedata")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpGet]
        public IActionResult Createfakedata()
        {

            for (int i = 0; i < 2000; i++)
            {
                unitOfWork.StockdefineRepository.Add(new StockdefineModel
                {
                    CreatedUser = "Fakedata",
                    CreateTime = DateTime.Now,
                    IsActive = true,
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    Departmentid = "fe2c453c-ce9c-47cd-acb9-16fd91c5a959",
                    Unitid = "76df918c-b078-44c5-9e0a-ac9340aae42a",
                    Name = Name.First()
                }); ;
            }
            unitOfWork.Complate();
            return Ok(FetchList());
        }
    }
}
