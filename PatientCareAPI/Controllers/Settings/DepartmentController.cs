using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PatientCareAPI.DataAccess;
using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PatientCareAPI.Controllers.Settings
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<DepartmentController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;
        public DepartmentController(IConfiguration configuration, ILogger<DepartmentController> logger, ApplicationDBContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
            unitOfWork = new UnitOfWork(context);
        }
        [Route("GetAll")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var items = unitOfWork.DepartmentRepository.GetAll().Where(u => u.IsActive).ToList();
            var stations = unitOfWork.StationsRepository.GetAll().Where(u => u.IsActive).ToList();
            foreach (var item in items)
            {
                var departmentstations = unitOfWork.DepartmenttoStationRepository.GetStationsbyDepartment(item.ConcurrencyStamp).ToList();
                foreach (var realstation in departmentstations)
                {
                    item.Stations.Add(stations.FirstOrDefault(u => u.ConcurrencyStamp == realstation));
                }
            }
            if (items.Count == 0)
                return NotFound();
            return Ok(items);
        }

        [Route("GetSelectedDepartment")]
        [HttpGet]
        public IActionResult GetSelectedCase(int ID)
        {
            var item = unitOfWork.DepartmentRepository.Getbyid(ID);
            var stations = unitOfWork.StationsRepository.GetAll().Where(u => u.IsActive).ToList();
            var departmentstations = unitOfWork.DepartmenttoStationRepository.GetStationsbyDepartment(item.ConcurrencyStamp).ToList();
            foreach (var realstation in departmentstations)
            {
                item.Stations.Add(stations.FirstOrDefault(u => u.ConcurrencyStamp == realstation));
            }
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [Route("Add")]
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
                unitOfWork.DepartmenttoStationRepository.AddStationstoDepartment(new DepartmenttoStationModel { DepartmentID = model.ConcurrencyStamp, StationID = item.ConcurrencyStamp });
            }
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Update")]
        [HttpPost]
        public IActionResult Update(DepartmentModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.DepartmentRepository.update(unitOfWork.DepartmentRepository.Getbyid(model.Id), model);
            unitOfWork.DepartmenttoStationRepository.RemoveStationsfromDepartment(model.ConcurrencyStamp);
            foreach (var item in model.Stations)
            {
                unitOfWork.DepartmenttoStationRepository.AddStationstoDepartment(new DepartmenttoStationModel { DepartmentID = model.ConcurrencyStamp, StationID = item.ConcurrencyStamp });
            }
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Delete")]
        [HttpDelete]
        public IActionResult Delete(DepartmentModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.DepartmentRepository.update(unitOfWork.DepartmentRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("DeleteFromDB")]
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
