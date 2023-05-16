using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PatientCareAPI.DataAccess;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Models.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using PatientCareAPI.Utils;
using PatientCareAPI.Models.Settings;
using Faker;

namespace PatientCareAPI.Controllers.Application
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientdefineController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<PatientdefineController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;
        Utilities Utilities;
        public PatientdefineController(IConfiguration configuration, ILogger<PatientdefineController> logger, ApplicationDBContext context)
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

        private List<PatientdefineModel> FetchList()
        {
            var List = unitOfWork.PatientdefineRepository.GetRecords<PatientdefineModel>(u => u.IsActive);
            foreach (var item in List)
            {
                item.Patienttype = unitOfWork.PatienttypeRepository.GetRecord<PatienttypeModel>(u => u.ConcurrencyStamp == item.Patienttypeid);
                item.Costumertype = unitOfWork.CostumertypeRepository.GetRecord<CostumertypeModel>(u => u.ConcurrencyStamp == item.Costumertypeid);
            }
            return List;
        }

        [HttpGet]
        [AuthorizeMultiplePolicy(UserAuthory.Patientdefine_Screen)]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(FetchList());
        }

        [Route("GetSelected")]
        [AuthorizeMultiplePolicy(UserAuthory.Patientdefine_Getselected)]
        [HttpGet]
        public IActionResult GetSelectedPatient(string guid)
        {
            var data = unitOfWork.PatientdefineRepository.GetRecord<PatientdefineModel>(u => u.ConcurrencyStamp == guid);
            data.Patienttype = unitOfWork.PatienttypeRepository.GetRecord<PatienttypeModel>(u => u.ConcurrencyStamp == data.Patienttypeid);
            data.Costumertype = unitOfWork.CostumertypeRepository.GetRecord<CostumertypeModel>(u => u.ConcurrencyStamp == data.Costumertypeid);
            return Ok(data);
        }

        [Route("Add")]
        [AuthorizeMultiplePolicy(UserAuthory.Patientdefine_Add)]
        [HttpPost]
        public IActionResult Add(PatientdefineModel model)
        {
            var username = GetSessionUser();
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.PatientdefineRepository.Add(model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy(UserAuthory.Patientdefine_Edit)]
        [HttpPost]
        public IActionResult Update(PatientdefineModel model)
        {
            var username = GetSessionUser();
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.PatientdefineRepository.update(unitOfWork.PatientdefineRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("Delete")]
        [AuthorizeMultiplePolicy(UserAuthory.Patientdefine_Delete)]
        [HttpPost]
        public IActionResult Delete(PatientdefineModel model)
        {
            var username = GetSessionUser();
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.PatientdefineRepository.update(unitOfWork.PatientdefineRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok(FetchList());
        }

        [Route("DeleteFromDB")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpDelete]
        public IActionResult DeleteFromDB(PatientdefineModel model)
        {
            unitOfWork.PatientdefineRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Createfakedata")]
        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpGet]
        public IActionResult Createfakedata()
        {

            for (int i = 0; i < 1000; i++)
            {
                unitOfWork.PatientdefineRepository.Add(new PatientdefineModel
                {
                    CreatedUser="Fakedata",
                    CreateTime=DateTime.Now,
                    IsActive=true,
                    ConcurrencyStamp=Guid.NewGuid().ToString(),
                    Firstname = Name.First(),
                    Lastname = Name.Last(),
                    Fathername = Name.FullName(),
                    Mothername = Name.FullName(),
                    Motherbiologicalaffinity = "ÖZ",
                    Ismotheralive = true,
                    Fatherbiologicalaffinity = "ÖZ",
                    Isfatheralive = true,
                    CountryID = Guid.NewGuid().ToString().Substring(0, 11),
                    Dateofbirth = DateTime.Now.AddYears(-30),
                    Placeofbirth = Address.City(),
                    Dateofdeath = null,
                    Placeofdeath = "",
                    Deathinfo="",
                    Gender="ERKEK",
                    Childnumber=3,
                    Costumertypeid = "d396336d-3b41-44af-a996-f1a2c0009474",
                    Patienttypeid = "0e411752-9739-47fd-9387-4e29ccb53e26",
                    Address1 = Address.StreetAddress(),
                    City = Address.City(),
                    Town = Address.StreetName(),
                    Country = Address.Country(),
                });;
            }
            unitOfWork.Complate();
            return Ok(FetchList());
        }
    }
}
