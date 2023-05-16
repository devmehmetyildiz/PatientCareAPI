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
    public class CheckperiodController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<CheckperiodController> _logger;
        private readonly ApplicationDBContext _context;
        Utilities Utilities;
        UnitOfWork unitOfWork;
        public CheckperiodController(IConfiguration configuration, ILogger<CheckperiodController> logger, ApplicationDBContext context)
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

        private List<CheckperiodModel> FetchList()
        {
            var List = unitOfWork.CheckperiodRepository.GetRecords<CheckperiodModel>(u => u.IsActive);
            foreach (var item in List)
            {
                var periodguids = unitOfWork.CheckperiodtoPeriodRepository.GetRecords<CheckperiodtoPeriodModel>(u => u.CheckperiodID == item.ConcurrencyStamp).Select(u => u.PeriodID).ToList();
                item.Periods = unitOfWork.PeriodRepository.GetPeriodsbyGuids(periodguids);
            }
            return List;
        }

        [Route("GetAll")]
        [AuthorizeMultiplePolicy(UserAuthory.Checkperiod_Screen)]
        [HttpGet]
        public IActionResult GetAll()
        {

            return Ok(FetchList());
        }

        [Route("GetSelected")]
        [AuthorizeMultiplePolicy((UserAuthory.Checkperiod_Getselected))]
        [HttpGet]
        public IActionResult GetSelected(string guid)
        {
            var Data = unitOfWork.CheckperiodRepository.GetRecord<CheckperiodModel>(u => u.ConcurrencyStamp == guid);
            if (Data == null)
            {
                return NotFound();
            }
            var periodguids = unitOfWork.CheckperiodtoPeriodRepository.GetRecords<CheckperiodtoPeriodModel>(u => u.CheckperiodID == Data.ConcurrencyStamp).Select(u => u.PeriodID).ToList();
            Data.Periods = unitOfWork.PeriodRepository.GetPeriodsbyGuids(periodguids);
            return Ok(Data);
        }

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Checkperiod_Add)]
        [HttpPost]
        public IActionResult Add(CheckperiodModel model)
        {
            var username = GetSessionUser();
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.CheckperiodRepository.Add(model);
            foreach (var item in model.Periods)
            {
                unitOfWork.CheckperiodtoPeriodRepository.Add(new CheckperiodtoPeriodModel { CheckperiodID = model.ConcurrencyStamp, PeriodID= item.ConcurrencyStamp });
            }
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy(UserAuthory.Checkperiod_Edit)]
        [HttpPost]
        public IActionResult Update(CheckperiodModel model)
        {
            var username = GetSessionUser();
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.CheckperiodRepository.update(unitOfWork.CheckperiodRepository.Getbyid(model.Id), model);
            unitOfWork.CheckperiodtoPeriodRepository.RemovePeriodsfromCheckperiod(model.ConcurrencyStamp);
            List<CheckperiodtoPeriodModel> list = new List<CheckperiodtoPeriodModel>();
            foreach (var item in model.Periods)
            {
                list.Add(new CheckperiodtoPeriodModel { CheckperiodID = model.ConcurrencyStamp, PeriodID = item.ConcurrencyStamp });
            }
            unitOfWork.CheckperiodtoPeriodRepository.AddRange(list);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Delete")]
        [AuthorizeMultiplePolicy(UserAuthory.Checkperiod_Delete)]
        [HttpPost]
        public IActionResult Delete(CheckperiodModel model)
        {
            //var periodsguids = unitOfWork.CheckperiodtoPeriodRepository.GetRecords<CheckperiodtoPeriodModel>(u => u.CheckperiodID == model.ConcurrencyStamp).Select(u => u.PeriodID).ToList();
            //var periods = unitOfWork.PeriodRepository.GetPeriodsbyGuids(periodsguids).Where(u => u.IsActive).ToList();
            //if (periods.Count > 0)
            //{
            //    return new ObjectResult(new ResponseModel { Status = "Can't Delete", Massage = model.Name + " kontrol grubuna bağlı kontroller var" }) { StatusCode = 403 };
            //}
            var username = GetSessionUser();
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.CheckperiodRepository.update(unitOfWork.CheckperiodRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("DeleteFromDB")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(DepartmentModel model)
        {
            unitOfWork.CheckperiodRepository.Remove(model.Id);
            unitOfWork.CheckperiodtoPeriodRepository.RemovePeriodsfromCheckperiod(model.ConcurrencyStamp);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
