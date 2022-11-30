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
    public class DepartmentController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<DepartmentController> _logger;
        private readonly ApplicationDBContext _context;
        Utilities Utilities;
        UnitOfWork unitOfWork;
        public DepartmentController(IConfiguration configuration, ILogger<DepartmentController> logger, ApplicationDBContext context)
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

        private List<DepartmentModel> FetchList()
        {
            var List = unitOfWork.DepartmentRepository.GetRecords<DepartmentModel>(u => u.IsActive);
            foreach (var item in List)
            {
                var stationguids = unitOfWork.DepartmenttoStationRepository.GetRecords<DepartmenttoStationModel>(u => u.DepartmentID == item.ConcurrencyStamp).Select(u => u.StationID).ToList();
                item.Stations = unitOfWork.StationsRepository.GetStationsbyGuids(stationguids);
            }
            return List;
        }

        [Route("GetAllSettings")]
        [AuthorizeMultiplePolicy(UserAuthory.Department_Screen)]
        [HttpGet]
        public IActionResult GetAllSettings()
        {
            return Ok(FetchList());
        }

        [Route("GetAll")]
        [AuthorizeMultiplePolicy(UserAuthory.Department_Screen)]
        [HttpGet]
        public IActionResult GetAll()
        {

            return Ok(FetchList());
        }

        [Route("GetSelected")]
        [AuthorizeMultiplePolicy((UserAuthory.Department_Screen + "," + UserAuthory.Department_Update))]
        [HttpGet]
        public IActionResult GetSelectedDepartment(string guid)
        {
            var Data = unitOfWork.DepartmentRepository.GetSingleRecord<DepartmentModel>(u=>u.ConcurrencyStamp==guid);
            if (Data == null)
            {
                return NotFound();
            }
            var stationguids = unitOfWork.DepartmenttoStationRepository.GetRecords<DepartmenttoStationModel>(u => u.DepartmentID == guid).Select(u => u.StationID).ToList();
            Data.Stations = unitOfWork.StationsRepository.GetStationsbyGuids(stationguids);
            return Ok(Data);
        }

       

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Department_Add)]
        [HttpPost]
        public IActionResult Add(DepartmentModel model)
        {
            var username = GetSessionUser();
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.DepartmentRepository.Add(model);
            List<DepartmenttoStationModel> list = new List<DepartmenttoStationModel>();
            foreach (var item in model.Stations)
            {
                list.Add(new DepartmenttoStationModel { DepartmentID = model.ConcurrencyStamp, StationID = item.ConcurrencyStamp });
            }
            unitOfWork.DepartmenttoStationRepository.AddRange(list);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy(UserAuthory.Department_Update)]
        [HttpPost]
        public IActionResult Update(DepartmentModel model)
        {
            var username = GetSessionUser();
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.DepartmentRepository.update(unitOfWork.DepartmentRepository.Getbyid(model.Id), model);
            unitOfWork.DepartmenttoStationRepository.RemoveStationsfromDepartment(model.ConcurrencyStamp);
            List<DepartmenttoStationModel> list = new List<DepartmenttoStationModel>();
            foreach (var item in model.Stations)
            {
                list.Add(new DepartmenttoStationModel { DepartmentID = model.ConcurrencyStamp, StationID = item.ConcurrencyStamp });
            }
            unitOfWork.DepartmenttoStationRepository.AddRange(list);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Delete")]
        [AuthorizeMultiplePolicy(UserAuthory.Department_Delete)]
        [HttpDelete]
        public IActionResult Delete(DepartmentModel model)
        {
            var username = GetSessionUser();
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.DepartmentRepository.update(unitOfWork.DepartmentRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("DeleteFromDB")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(DepartmentModel model)
        {
            unitOfWork.DepartmentRepository.Remove(model.Id);
            unitOfWork.DepartmenttoStationRepository.RemoveStationsfromDepartment(model.ConcurrencyStamp);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
