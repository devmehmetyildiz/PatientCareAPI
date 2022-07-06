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

        [Route("GetAll")]
        [Authorize(Roles = UserAuthory.Department_Screen)]
        [HttpGet]
        public IActionResult GetAll()
        {
            List<DepartmentModel> Data = new List<DepartmentModel>();
            if (Utilities.CheckAuth(UserAuthory.Department_ManageAll, this.User.Identity))
            {
                Data = unitOfWork.DepartmentRepository.GetAll().Where(u => u.IsActive).ToList();
                foreach (var item in Data)
                {
                    List<string> stations = unitOfWork.DepartmenttoStationRepository.GetAll().Where(u => u.DepartmentID == item.ConcurrencyStamp).Select(u => u.StationID).ToList();
                    item.Stations.AddRange(unitOfWork.StationsRepository.GetStationsbyGuids(stations));
                }
            }
            else
            {
                Data = unitOfWork.DepartmentRepository.GetAll().Where(u => u.IsActive && u.CreatedUser == this.User.Identity.Name).ToList();
                foreach (var item in Data)
                {
                    List<string> stations = unitOfWork.DepartmenttoStationRepository.GetAll().Where(u => u.DepartmentID == item.ConcurrencyStamp).Select(u => u.StationID).ToList();
                    item.Stations.AddRange(unitOfWork.StationsRepository.GetStationsbyGuids(stations));
                }
            }
            if (Data.Count == 0)
                return NotFound();
            return Ok(Data);
        }

        [Authorize(Roles = (UserAuthory.Department_Screen + "," + UserAuthory.Department_Update))]
        [Route("GetSelectedDepartment")]
        [HttpGet]
        public IActionResult GetSelectedCase(int ID)
        {
            var Data = unitOfWork.DepartmentRepository.Getbyid(ID);
            List<string> stations = unitOfWork.DepartmenttoStationRepository.GetAll().Where(u => u.DepartmentID == Data.ConcurrencyStamp).Select(u => u.StationID).ToList();
            Data.Stations.AddRange(unitOfWork.StationsRepository.GetStationsbyGuids(stations));
            if (Utilities.CheckAuth(UserAuthory.Department_ManageAll, this.User.Identity))
            {
                if (Data.CreatedUser == this.User.Identity.Name)
                {
                    return StatusCode(403);
                }
            }
            if (Data == null)
            {
                return NotFound();
            }
            return Ok(Data);
        }



        [Route("Add")]
        [Authorize(Roles = UserAuthory.Department_Add)]
        [HttpPost]
        public IActionResult Add(DepartmentModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.DepartmentRepository.Add(model);
            foreach (var item in model.Stations)
            {
                unitOfWork.DepartmenttoStationRepository.AddStationstoDepartment(new DepartmenttoStationModel {Id=0, DepartmentID = model.ConcurrencyStamp, StationID = item.ConcurrencyStamp });
            }
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Update")]
        [Authorize(Roles = UserAuthory.Department_Update)]
        [HttpPost]
        public IActionResult Update(DepartmentModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            if (!Utilities.CheckAuth(UserAuthory.Department_ManageAll, this.User.Identity))
            {
                if (model.CreatedUser == this.User.Identity.Name)
                {
                    return StatusCode(403);
                }
            }
            unitOfWork.DepartmentRepository.update(unitOfWork.DepartmentRepository.Getbyid(model.Id), model);
            unitOfWork.DepartmenttoStationRepository.RemoveStationsfromDepartment(model.ConcurrencyStamp);
            foreach (var item in model.Stations)
            {
                unitOfWork.DepartmenttoStationRepository.AddStationstoDepartment(new DepartmenttoStationModel {Id=0, DepartmentID = model.ConcurrencyStamp, StationID = item.ConcurrencyStamp });
            }
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Delete")]
        [Authorize(Roles = UserAuthory.Department_Delete)]
        [HttpDelete]
        public IActionResult Delete(DepartmentModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            if (!Utilities.CheckAuth(UserAuthory.Department_ManageAll, this.User.Identity))
            {
                if (model.CreatedUser == this.User.Identity.Name)
                {
                    return StatusCode(403);
                }
            }
            unitOfWork.DepartmentRepository.update(unitOfWork.DepartmentRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("DeleteFromDB")]
        [Authorize(Roles = UserAuthory.Admin)]
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
