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
    public class TodogroupdefineController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<TodogroupdefineController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;
        Utilities Utilities;
        public TodogroupdefineController(IConfiguration configuration, ILogger<TodogroupdefineController> logger, ApplicationDBContext context)
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

        private List<TodogroupdefineModel> FetchList()
        {
            var List = unitOfWork.TodogroupdefineRepository.GetRecords<TodogroupdefineModel>(u => u.IsActive);
            foreach (var item in List)
            {
                item.Department = unitOfWork.DepartmentRepository.GetRecord<DepartmentModel>(u => u.IsActive && u.ConcurrencyStamp == item.DepartmentID);
                var todoguids = unitOfWork.TodogrouptoTodoRepository.GetRecords<TodogrouptoTodoModel>(u => u.GroupID== item.ConcurrencyStamp).Select(u => u.TodoID).ToList();
                item.Todos = unitOfWork.TododefineRepository.GetTodosbyGuids(todoguids);
            }
            return List;
        }

        [Route("GetAll")]
        [AuthorizeMultiplePolicy(UserAuthory.Todogroupdefine_Screen)]
        [HttpGet]
        public IActionResult GetAll()
        {

            return Ok(FetchList());
        }

        [Route("GetSelected")]
        [AuthorizeMultiplePolicy((UserAuthory.Todogroupdefine_Getselected))]
        [HttpGet]
        public IActionResult GetSelected(string guid)
        {
            var Data = unitOfWork.TododefineRepository.GetRecord<TodogroupdefineModel>(u => u.ConcurrencyStamp == guid);
            if (Data == null)
            {
                return NotFound();
            }
            var todoguids = unitOfWork.TodogrouptoTodoRepository.GetRecords<TodogrouptoTodoModel>(u => u.GroupID == guid).Select(u => u.TodoID).ToList();
            Data.Todos = unitOfWork.TododefineRepository.GetTodosbyGuids(todoguids);
            Data.Department = unitOfWork.DepartmentRepository.GetRecord<DepartmentModel>(u => u.IsActive && u.ConcurrencyStamp == Data.ConcurrencyStamp);
            return Ok(Data);
        }

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Todogroupdefine_Add)]
        [HttpPost]
        public IActionResult Add(TodogroupdefineModel model)
        {
            var username = GetSessionUser();
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.TodogroupdefineRepository.Add(model);
            List<TodogrouptoTodoModel> list = new List<TodogrouptoTodoModel>();
            foreach (var item in model.Todos)
            {
                list.Add(new TodogrouptoTodoModel { GroupID = model.ConcurrencyStamp, TodoID= item.ConcurrencyStamp });
            }
            unitOfWork.TodogrouptoTodoRepository.AddRange(list);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy(UserAuthory.Todogroupdefine_Edit)]
        [HttpPost]
        public IActionResult Update(TodogroupdefineModel model)
        {
            var username = GetSessionUser();
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.TodogroupdefineRepository.update(unitOfWork.TodogroupdefineRepository.Getbyid(model.Id), model);
            unitOfWork.TodogrouptoTodoRepository.RemoveTodosfromTodogroup(model.ConcurrencyStamp);
            List<TodogrouptoTodoModel> list = new List<TodogrouptoTodoModel>();
            foreach (var item in model.Todos)
            {
                list.Add(new TodogrouptoTodoModel { GroupID = model.ConcurrencyStamp, TodoID = item.ConcurrencyStamp });
            }
            unitOfWork.TodogrouptoTodoRepository.AddRange(list);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Delete")]
        [AuthorizeMultiplePolicy(UserAuthory.Todogroupdefine_Delete)]
        [HttpPost]
        public IActionResult Delete(TodogroupdefineModel model)
        {
            var username = GetSessionUser();
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.TodogroupdefineRepository.update(unitOfWork.TodogroupdefineRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("DeleteFromDB")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(DepartmentModel model)
        {
            unitOfWork.TodogroupdefineRepository.Remove(model.Id);
            unitOfWork.TodogrouptoTodoRepository.RemoveTodosfromTodogroup(model.ConcurrencyStamp);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
