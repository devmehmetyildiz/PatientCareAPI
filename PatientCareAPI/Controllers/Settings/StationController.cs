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
    public class StationController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<StationController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;
        public StationController(IConfiguration configuration, ILogger<StationController> logger, ApplicationDBContext context)
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
            var items = unitOfWork.StationsRepository.GetAll().Where(u => u.IsActive).ToList();
            if (items.Count == 0)
                return NotFound();
            return Ok(items);
        }

        [Route("GetSelectedStation")]
        [HttpGet]
        public IActionResult GetSelectedStation(int ID)
        {
            var item = unitOfWork.StationsRepository.Getbyid(ID);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [Route("Add")]
        [HttpPost]
        public IActionResult Add(StationsModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.CreatedUser = username;
            model.NormalizedName = model.Name.ToUpper();
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.StationsRepository.Add(model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Update")]
        [HttpPost]
        public IActionResult Update(StationsModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.UpdatedUser = username;
            model.NormalizedName = model.Name.ToUpper();
            model.UpdateTime = DateTime.Now;
            unitOfWork.StationsRepository.update(unitOfWork.StationsRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Delete")]
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
        [HttpDelete]
        public IActionResult DeleteFromDB(DepartmentModel model)
        {
            unitOfWork.StationsRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
