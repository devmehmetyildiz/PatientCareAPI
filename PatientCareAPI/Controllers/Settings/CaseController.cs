using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PatientCareAPI.DataAccess;
using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Controllers.Settings
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class CaseController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;
        public CaseController(IConfiguration configuration, ILogger<AuthController> logger, ApplicationDBContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
            unitOfWork = new UnitOfWork(context);
        }
        [Route("GetAll")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var items = unitOfWork.CaseRepository.GetAll().Where(u=>u.IsActive).ToList();
            if (items.Count == 0)
                return NotFound();
            return  Ok(items);
        }

        [Route("GetSelectedCase")]
        [HttpGet]
        public IActionResult GetSelectedCase(int ID)
        {
            var item = unitOfWork.CaseRepository.Getbyid(ID);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [Route("Add")]
        [HttpPost]
        public IActionResult Add(CaseModel model)
        {
            model.NormalizedName = model.Name.ToUpper();
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.CaseRepository.Add(model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Update")]
        [HttpPost]
        public IActionResult Update(CaseModel model)
        {
            model.NormalizedName = model.Name.ToUpper();
            model.UpdateTime = DateTime.Now;
            unitOfWork.CaseRepository.update(unitOfWork.CaseRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Delete")]
        [HttpDelete]
        public IActionResult Delete(CaseModel model)
        {
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;           
            unitOfWork.CaseRepository.update(unitOfWork.CaseRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("DeleteFromDB")]
        [HttpDelete]
        public IActionResult DeleteFromDB(CaseModel model)
        {
            unitOfWork.CaseRepository.Remove(model.Id);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
