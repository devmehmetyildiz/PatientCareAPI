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
    [Route("api/[controller]")]
    [ApiController]
    public class PeriodController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<PeriodController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;
        Utilities Utilities;

        public PeriodController(IConfiguration configuration, ILogger<PeriodController> logger, ApplicationDBContext context)
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

        private List<PeriodModel> FetchList()
        {
            var List = unitOfWork.PeriodRepository.GetRecords<PeriodModel>(u => u.IsActive);
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
            var Data = unitOfWork.PeriodRepository.GetSingleRecord<PeriodModel>(u => u.ConcurrencyStamp == guid);
            if (Data == null)
            {
                return NotFound();
            }
            return Ok(Data);
        }

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Stations_Add)]
        [HttpPost]
        public IActionResult Add(PeriodModel model)
        {
            var username = GetSessionUser();
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.PeriodRepository.Add(model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy((UserAuthory.Stations_Update + "," + UserAuthory.Stations_Screen))]
        [HttpPost]
        public IActionResult Update(PeriodModel model)
        {
            var username = GetSessionUser();
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.PeriodRepository.update(unitOfWork.PeriodRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Delete")]
        [AuthorizeMultiplePolicy(UserAuthory.Stations_Delete)]
        [HttpPost]
        public IActionResult Delete(PeriodModel model)
        {
            var list = unitOfWork.CheckperiodtoPeriodRepository.GetRecords<CheckperiodtoPeriodModel>(u => u.PeriodID == model.ConcurrencyStamp).Select(u => u.CheckperiodID).ToList();
            var activelist = unitOfWork.CheckperiodRepository.GetCheckperiodsbyGuids(list).Where(u => u.IsActive).ToList();
            if (activelist.Count > 0)
            {
                return new ObjectResult(new ResponseModel { Status = "Can't Delete", Massage = model.Name + " kontrol grubu bağlı kontrol var" }) { StatusCode = 403 };
            }
            var username = GetSessionUser();
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.PeriodRepository.update(unitOfWork.PeriodRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("DeleteFromDB")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(PeriodModel model)
        {
            unitOfWork.PeriodRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
