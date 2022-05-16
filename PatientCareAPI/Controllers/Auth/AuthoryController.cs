using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PatientCareAPI.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Authentication;

namespace PatientCareAPI.Controllers.Auth
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthoryController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;

        public AuthoryController(IConfiguration configuration, ILogger<AuthController> logger, ApplicationDBContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
            unitOfWork = new UnitOfWork(context);
        }

        [Authorize]
        [Route("GetAll")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var authories = unitOfWork.AuthoryRepository.GetAll().Where(u => u.IsActive).ToList();
            var rolelist = unitOfWork.RolesRepository.GetAll();
            foreach (var authory in authories)
            {
                var roles = unitOfWork.AuthorytoRolesRepository.GetRolesByAuth(authory.ConcurrencyStamp);
                foreach (var item in roles)
                {
                    authory.Roles.Add(unitOfWork.RolesRepository.FindRoleBuGuid(item));
                }
            }
            return Ok(authories);
        }

        [Authorize]
        [Route("Add")]
        [HttpPost]
        public IActionResult Add(AuthoryModel model)
        {
            unitOfWork.AuthoryRepository.Add(model);
            foreach (var role in model.Roles)
            {
                unitOfWork.AuthorytoRolesRepository.AddRoletoAuth(new AuthorytoRoles { AuthoryID = model.ConcurrencyStamp, RoleID = role.ConcurrencyStamp });
            }
            return Ok();
        }

        [Authorize]
        [Route("Update")]
        [HttpPost]
        public IActionResult Update(AuthoryModel model)
        {
            unitOfWork.AuthoryRepository.update(unitOfWork.AuthoryRepository.Getbyid(model.Id), model);
            unitOfWork.AuthorytoRolesRepository.DeleteRolesbyAuth(model.ConcurrencyStamp);
            foreach (var role in model.Roles)
            {
                unitOfWork.AuthorytoRolesRepository.AddRoletoAuth(new AuthorytoRoles { AuthoryID = model.ConcurrencyStamp, RoleID = role.ConcurrencyStamp });
            }
            return Ok();
        }

        [Authorize]
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(AuthoryModel model)
        {
            unitOfWork.AuthoryRepository.Remove(model.Id);
            unitOfWork.AuthorytoRolesRepository.DeleteRolesbyAuth(model.ConcurrencyStamp);
            return Ok();
        }
    }
}
