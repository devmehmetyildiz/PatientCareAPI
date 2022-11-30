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
        [AuthorizeMultiplePolicy(UserAuthory.Stations_Screen)]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(FetchList());
        }

        [Route("GetSelected")]
        [AuthorizeMultiplePolicy(UserAuthory.Stations_Screen)]
        [HttpGet]
        public IActionResult GetSelectedStation(string guid)
        {
            var Data = unitOfWork.DepartmentRepository.GetSingleRecord<DepartmentModel>(u => u.ConcurrencyStamp == guid);
            if (Data == null)
            {
                return NotFound();
            }
            return Ok(Data);
        }

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Stations_Add)]
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
        [AuthorizeMultiplePolicy((UserAuthory.Stations_Update + "," + UserAuthory.Stations_Screen))]
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
        [AuthorizeMultiplePolicy(UserAuthory.Stations_Delete)]
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
