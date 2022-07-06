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
        [Route("GetAll")]
        [Authorize(Roles = UserAuthory.Stations_Screen)]
        [HttpGet]
        public IActionResult GetAll()
        {
            List<StationsModel> Data = new List<StationsModel>();
            if (Utilities.CheckAuth(UserAuthory.Stations_ManageAll, this.User.Identity))
            {
                Data = unitOfWork.StationsRepository.GetAll().Where(u => u.IsActive).ToList();
            }
            else
            {
                Data = unitOfWork.StationsRepository.GetAll().Where(u => u.IsActive && u.CreatedUser == this.User.Identity.Name).ToList();
            }
            if (Data.Count == 0)
            {
                return NotFound();
            }
            return Ok(Data);
        }

        [Route("GetSelectedStation")]
        [Authorize(Roles = UserAuthory.Stations_Screen)]
        [HttpGet]
        public IActionResult GetSelectedStation(int ID)
        {
            StationsModel Data = unitOfWork.StationsRepository.Getbyid(ID);
            if (!Utilities.CheckAuth(UserAuthory.Stations_ManageAll, this.User.Identity))
            {
                if (Data.CreatedUser == this.User.Identity.Name)
                {
                    return StatusCode(403);
                }
            }
            if (Data == null)
                return NotFound();
            return Ok(Data);
        }

        [Route("GetStationsByDepartments")]
        [Authorize(Roles = UserAuthory.Stations_Screen)]
        [HttpPost]
        public IActionResult GetStationsByDepartments(List<string> Departments)
        {
            List<StationsModel> stationsList = new List<StationsModel>();
            List<string> stations = new List<string>();
            foreach (var department in Departments)
            {
                stations.AddRange(unitOfWork.DepartmenttoStationRepository.GetStationsbyDepartment(department));
            }
            return Ok(unitOfWork.StationsRepository.GetStationsbyGuids(stations));
        }

        [Route("GetStationsByUser")]
        [Authorize(Roles = UserAuthory.Stations_Screen)]
        [HttpPost]
        public IActionResult GetStationsByUser(int ID)
        {
            var Departments = unitOfWork.UsertoDepartmentRepository.GetDepartmentsbyUser(unitOfWork.UsersRepository.Getbyid(ID).ConcurrencyStamp);
            List<StationsModel> stationsList = new List<StationsModel>();
            List<string> stations = new List<string>();
            foreach (var department in Departments)
            {
                stations.AddRange(unitOfWork.DepartmenttoStationRepository.GetStationsbyDepartment(department));
            }
            return Ok(unitOfWork.StationsRepository.GetStationsbyGuids(stations));
        }

        [Route("Add")]
        [Authorize(Roles = UserAuthory.Stations_Add)]
        [HttpPost]
        public IActionResult Add(StationsModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.StationsRepository.Add(model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Update")]
        [Authorize(Roles = (UserAuthory.Stations_Update + "," + UserAuthory.Stations_Screen))]
        [HttpPost]
        public IActionResult Update(StationsModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.StationsRepository.update(unitOfWork.StationsRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Delete")]
        [Authorize(Roles = UserAuthory.Stations_Delete)]
        [HttpDelete]
        public IActionResult Delete(StationsModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.StationsRepository.update(unitOfWork.StationsRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("DeleteFromDB")]
        [Authorize(Roles=UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(DepartmentModel model)
        {
            unitOfWork.StationsRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
