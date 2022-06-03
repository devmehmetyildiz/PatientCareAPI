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

        [Authorize]
        [Route("GetAll")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var roles = unitOfWork.RoleRepository.GetAll().Where(u => u.IsActive).ToList();
            var authorylist = unitOfWork.AuthoryRepository.GetAll();  //TODO unitofworkten cekiyor
            foreach (var role in roles)
            {
                var yetkis = unitOfWork.RoletoAuthoryRepository.GetAuthoriesByRole(role.ConcurrencyStamp);
                foreach (var item in yetkis)
                {
                    role.Yetkis.Add(unitOfWork.AuthoryRepository.FindAuthoryBuGuid(item));
                }
            }
            return Ok(roles);
        }

        [Authorize]
        [Route("GetSelectedAuthory")]
        [HttpGet]
        public IActionResult GetSelectedAuthory(int ID)
        {
            var role = unitOfWork.RoleRepository.Getbyid(ID);
            var authorylist = unitOfWork.AuthoryRepository.GetAll();  //TODO  unitofworkten cekiyor
            var authories = unitOfWork.RoletoAuthoryRepository.GetAuthoriesByRole(role.ConcurrencyStamp);
            foreach (var item in authories)
            {
                role.Yetkis.Add(unitOfWork.AuthoryRepository.FindAuthoryBuGuid(item));
            }
            return Ok(role);
        }

        [Authorize]
        [Route("GetAllroles")]
        [HttpGet]
        public IActionResult GetAllroles()
        {
            return Ok(unitOfWork.AuthoryRepository.GetAll());
        }

        [Authorize]
        [Route("Add")]
        [HttpPost]
        public IActionResult Add(RoleModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.CreatedUser = username;
            model.NormalizedName = model.Name.ToUpper();
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
            unitOfWork.RoleRepository.Add(model);
            foreach (var yetki in model.Yetkis)
            {
                unitOfWork.RoletoAuthoryRepository.AddAuthorytoRole(new RoletoAuthory { RoleID = model.ConcurrencyStamp, AuthoryID = yetki.ConcurrencyStamp });
            }
            unitOfWork.Complate();
            return Ok();
        }

        [Authorize]
        [Route("Update")]
        [HttpPost]
        public IActionResult Update(RoleModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.UpdatedUser = username;
            model.NormalizedName = model.Name.ToUpper();
            model.UpdateTime = DateTime.Now;
            unitOfWork.RoleRepository.update(unitOfWork.RoleRepository.Getbyid(model.Id), model);
            unitOfWork.RoletoAuthoryRepository.DeleteAuthoriesbyRole(model.ConcurrencyStamp);
            foreach (var yetki in model.Yetkis)
            {
                unitOfWork.RoletoAuthoryRepository.AddAuthorytoRole(new RoletoAuthory { RoleID = model.ConcurrencyStamp, AuthoryID = yetki.ConcurrencyStamp });
            }
            unitOfWork.Complate();
            return Ok();
        }

        [Authorize]
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(RoleModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.RoleRepository.Remove(model.Id);
            unitOfWork.RoletoAuthoryRepository.DeleteAuthoriesbyRole(model.ConcurrencyStamp);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
