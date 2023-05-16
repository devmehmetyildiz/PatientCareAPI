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
    public class TododefineController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<TododefineController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;
        Utilities Utilities;
        public TododefineController(IConfiguration configuration, ILogger<TododefineController> logger, ApplicationDBContext context)
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

        private List<TododefineModel> FetchList()
        {
            var List = unitOfWork.TododefineRepository.GetRecords<TododefineModel>(u => u.IsActive);
            foreach (var item in List)
            {
                var perodguids = unitOfWork.TododefinetoPeriodRepository.GetRecords<TododefinetoPeriodModel>(u => u.TododefineID == item.ConcurrencyStamp).Select(u => u.PeriodID).ToList();
                item.Periods = unitOfWork.PeriodRepository.GetPeriodsbyGuids(perodguids);
            }
            return List;
        }

        [Route("GetAll")]
        [AuthorizeMultiplePolicy(UserAuthory.Tododefine_Screen)]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(FetchList());
        }

        [Route("GetSelected")]
        [AuthorizeMultiplePolicy(UserAuthory.Tododefine_Getselected)]
        [HttpGet]
        public IActionResult GetSelected(string guid)
        {
            var Data = unitOfWork.TododefineRepository.GetRecord<TododefineModel>(u => u.ConcurrencyStamp == guid);
            var perodguids = unitOfWork.TododefinetoPeriodRepository.GetRecords<TododefinetoPeriodModel>(u => u.TododefineID == Data.ConcurrencyStamp).Select(u => u.PeriodID).ToList();
            Data.Periods = unitOfWork.PeriodRepository.GetPeriodsbyGuids(perodguids);
            if (Data == null)
            {
                return NotFound();
            }
            return Ok(Data);
        }

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Tododefine_Add)]
        [HttpPost]
        public IActionResult Add(TododefineModel model)
        {
            var username = GetSessionUser();
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.TododefineRepository.Add(model);
            foreach (var item in model.Periods)
            {
                unitOfWork.TododefinetoPeriodRepository.Add(new TododefinetoPeriodModel { PeriodID = item.ConcurrencyStamp, TododefineID = model.ConcurrencyStamp });
            }
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy((UserAuthory.Tododefine_Edit))]
        [HttpPost]
        public IActionResult Update(TododefineModel model)
        {
            var username = GetSessionUser();
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.TododefineRepository.update(unitOfWork.TododefineRepository.Getbyid(model.Id), model);
            unitOfWork.TododefinetoPeriodRepository.RemovePeriodsfromTododefines(model.ConcurrencyStamp);
            foreach (var item in model.Periods)
            {
                unitOfWork.TododefinetoPeriodRepository.Add(new TododefinetoPeriodModel { PeriodID = item.ConcurrencyStamp, TododefineID = model.ConcurrencyStamp });
            }
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Delete")]
        [AuthorizeMultiplePolicy(UserAuthory.Tododefine_Delete)]
        [HttpPost]
        public IActionResult Delete(TododefineModel model)
        {
            var list = unitOfWork.TodogrouptoTodoRepository.GetRecords<TodogrouptoTodoModel>(u => u.TodoID == model.ConcurrencyStamp).Select(u => u.GroupID).ToList();
            var activelist = unitOfWork.TodogroupdefineRepository.GetGroupsbyGuids(list).Where(u => u.IsActive).ToList();
            if (activelist.Count > 0)
            {
                return new ObjectResult(new ResponseModel { Status = "Can't Delete", Massage = model.Name + " yapılaklara bağlı gruplar var" }) { StatusCode = 403 };
            }
            var username = GetSessionUser();
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.TododefineRepository.update(unitOfWork.TododefineRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("DeleteFromDB")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(DepartmentModel model)
        {
            unitOfWork.TododefineRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
