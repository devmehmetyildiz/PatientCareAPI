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
    public class PatientmovementController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<PatientmovementController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;
        Utilities Utilities;
        public PatientmovementController(IConfiguration configuration, ILogger<PatientmovementController> logger, ApplicationDBContext context)
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

        private List<PatientmovementModel> FetchList()
        {
            var List = unitOfWork.PatientmovementRepository.GetRecords<PatientmovementModel>(u => u.IsActive);
            foreach (var item in List)
            {
                item.Patient = unitOfWork.PatientRepository.GetRecord<PatientModel>(u => u.ConcurrencyStamp == item.PatientID);
                if (item.Patient != null)
                {
                    item.Patient.Patientdefine = unitOfWork.PatientdefineRepository.GetRecord<PatientdefineModel>(u => u.ConcurrencyStamp == item.Patient.PatientdefineID);
                }
            }
            return List;
        }

        [HttpGet]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Screen)]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(FetchList());
        }

        [Route("GetSelected")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Screen)]
        [HttpGet]
        public IActionResult GetSelected(string guid)
        {
            var data = unitOfWork.PatientmovementRepository.GetRecord<PatientmovementModel>(u => u.ConcurrencyStamp == guid);
            data.Patient = unitOfWork.PatientRepository.GetRecord<PatientModel>(u => u.ConcurrencyStamp == data.PatientID);
            if (data.Patient != null)
            {
                data.Patient.Patientdefine = unitOfWork.PatientdefineRepository.GetRecord<PatientdefineModel>(u => u.ConcurrencyStamp == data.Patient.PatientdefineID);
            }
            return Ok(data);
        }

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Screen)]
        [HttpPost]
        public IActionResult Add(PatientmovementModel model)
        {
            var username = GetSessionUser();
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.PatientmovementRepository.Add(model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Screen)]
        [HttpPost]
        public IActionResult Update(PatientmovementModel model)
        {
            var username = GetSessionUser();
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.PatientmovementRepository.update(unitOfWork.PatientmovementRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Delete")]
        [AuthorizeMultiplePolicy(UserAuthory.Patients_Screen)]
        [HttpPost]
        public IActionResult Delete(PatientmovementModel model)
        {
            var username = GetSessionUser();
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.PatientmovementRepository.update(unitOfWork.PatientmovementRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("DeleteFromDB")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(PatientmovementModel model)
        {
            unitOfWork.PatientmovementRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
