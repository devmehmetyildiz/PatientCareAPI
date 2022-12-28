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
using PatientCareAPI.Utils;

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

        private string GetSessionUser()
        {
            return (this.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name)?.Value;
        }

        [AuthorizeMultiplePolicy(UserAuthory.Roles_Screen)]
        [Route("GetAll")]
        [HttpGet]
        public IActionResult RolesGetAll()
        {
            var roles = unitOfWork.RoleRepository.GetAll().Where(u => u.IsActive).ToList();
            foreach (var role in roles)
            {
                List<string> authories = unitOfWork.RoletoAuthoryRepository.GetRecords<RoletoAuthory>(u => u.RoleID == role.ConcurrencyStamp).Select(u => u.AuthoryID).ToList();
                role.Authories.AddRange(unitOfWork.AuthoryRepository.GetAuthoriesbyGuids(authories));
            }
            return Ok(roles);
        }

        [AuthorizeMultiplePolicy(UserAuthory.Roles_Screen)]
        [Route("Getselected")]
        [HttpGet]
        public IActionResult RolesGetselected(string guid)
        {
            var role = unitOfWork.RoleRepository.GetSingleRecord<RoleModel>(u=>u.ConcurrencyStamp==guid);
            List<string> authories = unitOfWork.RoletoAuthoryRepository.GetRecords<RoletoAuthory>(u => u.RoleID == role.ConcurrencyStamp).Select(u => u.AuthoryID).ToList();
            role.Authories.AddRange(unitOfWork.AuthoryRepository.GetAuthoriesbyGuids(authories));
            return Ok(role);
        }

        [AuthorizeMultiplePolicy(UserAuthory.Roles_Screen)]
        [Route("GetAllAuthories")]
        [HttpGet]
        public IActionResult RolesGetAllroles()
        {
            return Ok(unitOfWork.AuthoryRepository.GetRecords<AuthoryModel>(u => u.Name != UserAuthory.Admin).OrderBy(u => u.Group));
        }

        [AuthorizeMultiplePolicy(UserAuthory.Roles_Screen)]
        [Route("GetAllAuthoryGroups")]
        [HttpGet]
        public IActionResult RolesGetAllAuthoryGroups()
        {
            return Ok(unitOfWork.AuthoryRepository.GetRecords<AuthoryModel>(u => u.Name != UserAuthory.Admin).OrderBy(u => u.Group).Select(u=>u.Group).Distinct());
        }

        [AuthorizeMultiplePolicy(UserAuthory.Roles_Screen + "," + UserAuthory.Roles_Add)]
        [Route("Add")]
        [HttpPost]
        public IActionResult RolesAdd(RoleModel model)
        {
            model.CreatedUser = GetSessionUser();
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.RoleRepository.Add(model);
            foreach (var yetki in model.Authories)
            {
                unitOfWork.RoletoAuthoryRepository.AddAuthorytoRole(new RoletoAuthory { RoleID = model.ConcurrencyStamp, AuthoryID = yetki.ConcurrencyStamp });
            }
            unitOfWork.Complate();
            var roles = unitOfWork.RoleRepository.GetAll().Where(u => u.IsActive).ToList();
            foreach (var role in roles)
            {
                List<string> authories = unitOfWork.RoletoAuthoryRepository.GetRecords<RoletoAuthory>(u => u.RoleID == role.ConcurrencyStamp).Select(u => u.AuthoryID).ToList();
                role.Authories.AddRange(unitOfWork.AuthoryRepository.GetAuthoriesbyGuids(authories));
            }
            return Ok(roles);
        }

        [AuthorizeMultiplePolicy(UserAuthory.Roles_Screen+","+UserAuthory.Roles_Update)]
        [Route("Update")]
        [HttpPost]
        public IActionResult RolesUpdate(RoleModel model)
        {
            model.UpdatedUser = (this.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name)?.Value;
            model.UpdateTime = DateTime.Now;
            unitOfWork.RoleRepository.update(unitOfWork.RoleRepository.Getbyid(model.Id), model);
            unitOfWork.RoletoAuthoryRepository.DeleteAuthoriesbyRole(model.ConcurrencyStamp);
            foreach (var yetki in model.Authories)
            {
                unitOfWork.RoletoAuthoryRepository.AddAuthorytoRole(new RoletoAuthory { RoleID = model.ConcurrencyStamp, AuthoryID = yetki.ConcurrencyStamp });
            }
            unitOfWork.Complate();
            var roles = unitOfWork.RoleRepository.GetAll().Where(u => u.IsActive).ToList();
            foreach (var role in roles)
            {
                List<string> authories = unitOfWork.RoletoAuthoryRepository.GetRecords<RoletoAuthory>(u => u.RoleID == role.ConcurrencyStamp).Select(u => u.AuthoryID).ToList();
                role.Authories.AddRange(unitOfWork.AuthoryRepository.GetAuthoriesbyGuids(authories));
            }
            return Ok(roles);
        }

        [AuthorizeMultiplePolicy(UserAuthory.Roles_Screen + "," + UserAuthory.Roles_Delete)]
        [Route("Delete")]
        [HttpPost]
        public IActionResult RolesDelete(RoleModel model)
        {
            model.DeleteUser = GetSessionUser();
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.RoleRepository.update(unitOfWork.RoleRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            var roles = unitOfWork.RoleRepository.GetAll().Where(u => u.IsActive).ToList();
            foreach (var role in roles)
            {
                List<string> authories = unitOfWork.RoletoAuthoryRepository.GetRecords<RoletoAuthory>(u => u.RoleID == role.ConcurrencyStamp).Select(u => u.AuthoryID).ToList();
                role.Authories.AddRange(unitOfWork.AuthoryRepository.GetAuthoriesbyGuids(authories));
            }
            return Ok(roles);
        }

        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [Route("DeleteFromDB")]
        [HttpDelete]
        public IActionResult RolesDeleteFromDB(RoleModel model)
        {
            unitOfWork.RoleRepository.Remove(model.Id);
            unitOfWork.RoletoAuthoryRepository.DeleteAuthoriesbyRole(model.ConcurrencyStamp);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
