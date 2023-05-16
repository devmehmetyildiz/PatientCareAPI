using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PatientCareAPI.DataAccess;
using PatientCareAPI.Models.Application;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Models.Settings;
using PatientCareAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PatientCareAPI.Controllers.Application
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<TodoController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;
        Utilities Utilities;
        public TodoController(IConfiguration configuration, ILogger<TodoController> logger, ApplicationDBContext context)
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

        private List<TodoModel> FetchList()
        {
            var List = unitOfWork.TodoRepository.GetRecords<TodoModel>(u => u.IsActive);
            foreach (var item in List)
            {
                item.Movement = unitOfWork.PatientmovementRepository.GetRecord<PatientmovementModel>(u => u.ConcurrencyStamp == item.MovementID);
                if (item.Movement != null)
                {
                    item.Patient = unitOfWork.PatientRepository.GetRecord<PatientModel>(u => u.ConcurrencyStamp == item.Movement.PatientID);
                    if (item.Patient != null)
                    {
                        item.Patient.Patientdefine = unitOfWork.PatientdefineRepository.GetRecord<PatientdefineModel>(u => u.ConcurrencyStamp == item.Patient.PatientdefineID);
                    }
                }
                item.Tododefine = unitOfWork.TododefineRepository.GetRecord<TododefineModel>(u => u.ConcurrencyStamp == item.TododefineID);
            }
            return List;
        }

        [HttpGet]
        [AuthorizeMultiplePolicy(UserAuthory.Case_Screen)]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(FetchList());
        }

        [Route("GetSelected")]
        [AuthorizeMultiplePolicy(UserAuthory.Case_Getselected)]
        [HttpGet]
        public IActionResult GetSelectedCase(string guid)
        {
            var Data = unitOfWork.TodoRepository.GetRecord<TodoModel>(u => u.IsActive);
            Data.Movement = unitOfWork.PatientmovementRepository.GetRecord<PatientmovementModel>(u => u.ConcurrencyStamp == Data.MovementID);
            if (Data.Movement != null)
            {
                Data.Patient = unitOfWork.PatientRepository.GetRecord<PatientModel>(u => u.ConcurrencyStamp == Data.Movement.PatientID);
                if (Data.Patient != null)
                {
                    Data.Patient.Patientdefine = unitOfWork.PatientdefineRepository.GetRecord<PatientdefineModel>(u => u.ConcurrencyStamp == Data.Patient.PatientdefineID);
                }
            }
            Data.Tododefine = unitOfWork.TododefineRepository.GetRecord<TododefineModel>(u => u.ConcurrencyStamp == Data.TododefineID);
            return Ok(Data);
        }

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Case_Add)]
        [HttpPost]
        public IActionResult Add(TodoModel model)
        {
            var username = GetSessionUser();
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.TodoRepository.Add(model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy(UserAuthory.Case_Edit)]
        [HttpPost]
        public IActionResult Update(TodoModel model)
        {
            var username = GetSessionUser();
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.TodoRepository.update(unitOfWork.TodoRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Delete")]
        [AuthorizeMultiplePolicy(UserAuthory.Case_Delete)]
        [HttpPost]
        public IActionResult Delete(TodoModel model)
        {
            var username = GetSessionUser();
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.TodoRepository.update(unitOfWork.TodoRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("DeleteFromDB")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(CaseModel model)
        {
            unitOfWork.TodoRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
