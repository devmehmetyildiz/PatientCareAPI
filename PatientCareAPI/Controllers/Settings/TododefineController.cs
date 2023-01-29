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
        public IActionResult GetSelected(string guid)
        {
            var Data = unitOfWork.TododefineRepository.GetSingleRecord<TododefineModel>(u => u.ConcurrencyStamp == guid);
            if (Data == null)
            {
                return NotFound();
            }
            return Ok(Data);
        }

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Stations_Add)]
        [HttpPost]
        public IActionResult Add(TododefineModel model)
        {
            var username = GetSessionUser();
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.TododefineRepository.Add(model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy((UserAuthory.Stations_Update + "," + UserAuthory.Stations_Screen))]
        [HttpPost]
        public IActionResult Update(TododefineModel model)
        {
            var username = GetSessionUser();
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.TododefineRepository.update(unitOfWork.TododefineRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Delete")]
        [AuthorizeMultiplePolicy(UserAuthory.Stations_Delete)]
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
