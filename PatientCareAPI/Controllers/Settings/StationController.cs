using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PatientCareAPI.DataAccess;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Models.Settings;
using PatientCareAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PatientCareAPI.Controllers.Settings
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StationController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<StationController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;
        Utilities Utilities;

        public StationController(IConfiguration configuration, ILogger<StationController> logger, ApplicationDBContext context)
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

        private List<StationsModel> FetchList()
        {
            var List = unitOfWork.StationsRepository.GetRecords<StationsModel>(u => u.IsActive);
            return List;
        }

        [Route("GetAll")]
        [AuthorizeMultiplePolicy(UserAuthory.Station_Screen)]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(FetchList());
        }

        [Route("GetSelected")]
        [AuthorizeMultiplePolicy(UserAuthory.Station_Getselected)]
        [HttpGet]
        public IActionResult GetSelectedStation(string guid)
        {
            var Data = unitOfWork.StationsRepository.GetRecord<StationsModel>(u => u.ConcurrencyStamp == guid);
            if (Data == null)
            {
                return NotFound();
            }
            return Ok(Data);
        }

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Station_Add)]
        [HttpPost]
        public IActionResult Add(StationsModel model)
        {
            var username = GetSessionUser();
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.StationsRepository.Add(model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy((UserAuthory.Station_Edit))]
        [HttpPost]
        public IActionResult Update(StationsModel model)
        {
            var username = GetSessionUser();
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.StationsRepository.update(unitOfWork.StationsRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Delete")]
        [AuthorizeMultiplePolicy(UserAuthory.Station_Delete)]
        [HttpPost]
        public IActionResult Delete(StationsModel model)
        {
            var list = unitOfWork.DepartmenttoStationRepository.GetRecords<DepartmenttoStationModel>(u => u.StationID == model.ConcurrencyStamp).Select(u=>u.DepartmentID).ToList();
            var activelist = unitOfWork.DepartmentRepository.GetDepartmentsbyGuids(list).Where(u=>u.IsActive).ToList();
            if (activelist.Count > 0)
            {
                return new ObjectResult(new ResponseModel { Status = "Can't Delete", Massage = model.Name +" istasyona bağlı departmanlar var" }) { StatusCode = 403 };
            }
            var username = GetSessionUser();
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.StationsRepository.update(unitOfWork.StationsRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("DeleteFromDB")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(DepartmentModel model)
        {
            unitOfWork.StationsRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
