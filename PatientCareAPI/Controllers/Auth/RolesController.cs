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
    public class RolesController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;

        public RolesController(IConfiguration configuration, ILogger<AuthController> logger, ApplicationDBContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
            unitOfWork = new UnitOfWork(context);
        }

        [Authorize(Roles = UserAuthory.Roles_Screen)]
        [Route("GetAll")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var roles = unitOfWork.RoleRepository.GetAll().Where(u => u.IsActive).ToList();
            foreach (var role in roles)
            {
                List<string> authories = unitOfWork.RoletoAuthoryRepository.GetAll().Where(u => u.RoleID== role.ConcurrencyStamp).Select(u => u.AuthoryID).ToList();
                role.Authories.AddRange(unitOfWork.AuthoryRepository.GetAuthoriesbyGuids(authories));
            }
            return Ok(roles);
        }

        [Authorize(Roles = UserAuthory.Roles_Screen)]
        [Route("GetSelectedRole")]
        [HttpGet]
        public IActionResult GetSelectedRole(int ID)
        {
            var role = unitOfWork.RoleRepository.Getbyid(ID);
            List<string> authories = unitOfWork.RoletoAuthoryRepository.GetAll().Where(u => u.RoleID == role.ConcurrencyStamp).Select(u => u.AuthoryID).ToList();
            role.Authories.AddRange(unitOfWork.AuthoryRepository.GetAuthoriesbyGuids(authories));
            return Ok(role);
        }

        [Authorize(Roles = UserAuthory.Roles_Screen)]
        [Route("GetAllAuthories")]
        [HttpGet]
        public IActionResult GetAllroles()
        {
            return Ok(unitOfWork.AuthoryRepository.GetAll().Where(U=>U.Name!=UserAuthory.Admin).ToList().OrderBy(u=>u.Group));
        }

        [Authorize(Roles = UserAuthory.Roles_Add)]
        [Route("Add")]
        [HttpPost]
        public IActionResult Add(RoleModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.RoleRepository.Add(model);
            foreach (var yetki in model.Authories)
            {
                unitOfWork.RoletoAuthoryRepository.AddAuthorytoRole(new RoletoAuthory { RoleID = model.ConcurrencyStamp, AuthoryID = yetki.ConcurrencyStamp });
            }
            unitOfWork.Complate();
            return Ok();
        }

        [Authorize(Roles = (UserAuthory.Roles_Screen + "," + UserAuthory.Roles_Update))]
        [Route("Update")]
        [HttpPost]
        public IActionResult Update(RoleModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.RoleRepository.update(unitOfWork.RoleRepository.Getbyid(model.Id), model);
            unitOfWork.RoletoAuthoryRepository.DeleteAuthoriesbyRole(model.ConcurrencyStamp);
            foreach (var yetki in model.Authories)
            {
                unitOfWork.RoletoAuthoryRepository.AddAuthorytoRole(new RoletoAuthory { RoleID = model.ConcurrencyStamp, AuthoryID = yetki.ConcurrencyStamp });
            }
            unitOfWork.Complate();
            return Ok();
        }

        [Authorize(Roles = UserAuthory.Roles_Delete)]
        [Route("Delete")]
        [HttpDelete]
        public IActionResult Delete(RoleModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.RoleRepository.update(unitOfWork.RoleRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok();
        }

        [Authorize(Roles = UserAuthory.Admin)]
        [Route("DeleteFromDB")]
        [HttpDelete]
        public IActionResult DeleteFromDB(RoleModel model)
        {
            unitOfWork.RoleRepository.Remove(model.Id);
            unitOfWork.RoletoAuthoryRepository.DeleteAuthoriesbyRole(model.ConcurrencyStamp);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
