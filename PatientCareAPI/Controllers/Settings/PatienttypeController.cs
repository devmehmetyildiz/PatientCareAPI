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

namespace PatientCareAPI.Controllers.Settings
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PatienttypeController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<PatienttypeController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;
        Utilities Utilities;
        public PatienttypeController(IConfiguration configuration, ILogger<PatienttypeController> logger, ApplicationDBContext context)
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

        private List<PatienttypeModel> FetchList()
        {
            var List = unitOfWork.PatienttypeRepository.GetRecords<PatienttypeModel>(u => u.IsActive);
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
            var Data = unitOfWork.PatienttypeRepository.GetSingleRecord<PatienttypeModel>(u => u.ConcurrencyStamp == guid);
            if (Data == null)
            {
                return NotFound();
            }
            return Ok(Data);
        }


        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Stations_Add)]
        [HttpPost]
        public IActionResult Add(PatienttypeModel model)
        {
            var username = GetSessionUser();
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.PatienttypeRepository.Add(model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy((UserAuthory.Stations_Update + "," + UserAuthory.Stations_Screen))]
        [HttpPost]
        public IActionResult Update(PatienttypeModel model)
        {
            var username = GetSessionUser();
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.PatienttypeRepository.update(unitOfWork.PatienttypeRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Delete")]
        [AuthorizeMultiplePolicy(UserAuthory.Stations_Delete)]
        [HttpPost]
        public IActionResult Delete(PatienttypeModel model)
        {
            var username = GetSessionUser();
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.PatienttypeRepository.update(unitOfWork.PatienttypeRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("DeleteFromDB")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(PatienttypeModel model)
        {
            unitOfWork.PatienttypeRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
