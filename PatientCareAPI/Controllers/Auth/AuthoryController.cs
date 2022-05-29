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
using System.Security.Claims;

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
            var rolelist = unitOfWork.RolesRepository.GetAll();  //TODO unitofworkten cekiyor
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
        [Route("GetSelectedAuthory")]
        [HttpGet]
        public IActionResult GetSelectedAuthory(int ID)
        {
            var authory = unitOfWork.AuthoryRepository.Getbyid(ID);
            var rolelist = unitOfWork.RolesRepository.GetAll();  //TODO  unitofworkten cekiyor
            var roles = unitOfWork.AuthorytoRolesRepository.GetRolesByAuth(authory.ConcurrencyStamp);
            foreach (var item in roles)
            {
                authory.Roles.Add(unitOfWork.RolesRepository.FindRoleBuGuid(item));
            }
            return Ok(authory);
        }

        [Authorize]
        [Route("GetAllroles")]
        [HttpGet]
        public IActionResult GetAllroles()
        {
            return Ok(unitOfWork.RolesRepository.GetAll());
        }

        [Authorize]
        [Route("Add")]
        [HttpPost]
        public IActionResult Add(AuthoryModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.CreatedUser = username;
            model.NormalizedName = model.Name.ToUpper();
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.AuthoryRepository.Add(model);
            foreach (var role in model.Roles)
            {
                unitOfWork.AuthorytoRolesRepository.AddRoletoAuth(new AuthorytoRoles { AuthoryID = model.ConcurrencyStamp, RoleID = role.ConcurrencyStamp });
            }
            unitOfWork.Complate();
            return Ok();
        }

        [Authorize]
        [Route("Update")]
        [HttpPost]
        public IActionResult Update(AuthoryModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.UpdatedUser = username;
            model.NormalizedName = model.Name.ToUpper();
            model.UpdateTime = DateTime.Now;
            unitOfWork.AuthoryRepository.update(unitOfWork.AuthoryRepository.Getbyid(model.Id), model);
            unitOfWork.AuthorytoRolesRepository.DeleteRolesbyAuth(model.ConcurrencyStamp);
            foreach (var role in model.Roles)
            {
                unitOfWork.AuthorytoRolesRepository.AddRoletoAuth(new AuthorytoRoles { AuthoryID = model.ConcurrencyStamp, RoleID = role.ConcurrencyStamp });
            }
            unitOfWork.Complate();
            return Ok();
        }

        [Authorize]
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(AuthoryModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.AuthoryRepository.Remove(model.Id);
            unitOfWork.AuthorytoRolesRepository.DeleteRolesbyAuth(model.ConcurrencyStamp);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
