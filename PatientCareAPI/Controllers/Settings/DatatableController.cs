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
    public class DatatableController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<DatatableController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;
        Utilities Utilities;
        public DatatableController(IConfiguration configuration, ILogger<DatatableController> logger, ApplicationDBContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
            Utilities = new Utilities(context);
            unitOfWork = new UnitOfWork(context);
        }

        [HttpGet]
        [AuthorizeMultiplePolicy(UserAuthory.Basic)]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            List<DatatableModel> Data = new List<DatatableModel>();
            Data = unitOfWork.DatatableRepository.GetAll().ToList();
            if (Data.Count == 0)
            {
                return NotFound();
            }
            return Ok(Data);
        }

        [HttpPost]
        [AuthorizeMultiplePolicy(UserAuthory.Basic)]
        [Route("Add")]
        public IActionResult Add(DatatableModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.DatatableRepository.Add(model);
            unitOfWork.Complate();
            return Ok();
        }

        [HttpPost]
        [AuthorizeMultiplePolicy(UserAuthory.Basic)]
        [Route("AddRecord")]
        public IActionResult Add(List<DatatableModel> list)
        {
            var username = (this.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name)?.Value;
            foreach (var model in list)
            {
                model.CreatedUser = username;
                model.IsActive = true;
                model.CreateTime = DateTime.Now;
                model.ConcurrencyStamp = Guid.NewGuid().ToString();
            }
            unitOfWork.DatatableRepository.AddRange(list);
            unitOfWork.Complate();
            return Ok();
        }

        [Route("Update")]
        [AuthorizeMultiplePolicy(UserAuthory.Basic)]
        [HttpPost]
        public IActionResult Update(DatatableModel model)
        {
            var username = (this.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name)?.Value;
            DatatableModel newmodel = unitOfWork.DatatableRepository.GetDatatableByName(model.Tablename);
            newmodel.Json = model.Json;
            newmodel.UpdatedUser = username;
            newmodel.UpdateTime = DateTime.Now;
            unitOfWork.DatatableRepository.update(unitOfWork.DatatableRepository.GetDatatableByName(model.Tablename), newmodel);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
