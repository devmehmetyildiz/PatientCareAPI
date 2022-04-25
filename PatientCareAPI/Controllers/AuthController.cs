using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.DataAccess;
using Microsoft.Extensions.Logging;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Utils;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace PatientCareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;
        CryptographyProcessor securityutils;
        
        public AuthController(IConfiguration configuration, ILogger<AuthController> logger,ApplicationDBContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
            unitOfWork = new UnitOfWork(context);          
            securityutils = new CryptographyProcessor();
        
           
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExist = unitOfWork.UsersRepository.FindUserByName(model.Username);
            if (userExist != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Error", Massage = "Geçerli Bir Kullanıcı Adı Giriniz" });
            var userGuid = Guid.NewGuid().ToString();
            var salt = securityutils.CreateSalt(30);
            unitOfWork.UsertoSaltRepository.Add(new UsertoSaltModel { Salt = salt, UserID = userGuid });
            UsersModel user = new UsersModel()
            {
               Id=0,
               Username =  model.Username,
               NormalizedUsername = model.Username.ToUpper(),
               ConcurrencyStamp =userGuid,
               Email = model.Email,
               AccessFailedCount = 0,
               EmailConfirmed = false,
               Isactive = false,
               PasswordHash = securityutils.GenerateHash(model.Password,salt),
               PhoneNumber = "",
               PhoneNumberConfirmed = false               
            };
            unitOfWork.UsersRepository.Add(user);
            AddRoleToUser(UserRoles.Basic, user);
            unitOfWork.Complate();
            return Ok(new ResponseModel { Status = "Success", Massage = "Kullanıcı Başarı ile Oluşturuldu" });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = unitOfWork.UsersRepository.FindUserByName(model.Username);
            if ((user == null) && !CheckPassword(user, model.Password))
            {
                return Unauthorized();
            }            
            var authClaims = new List<Claim>
                {
                     new Claim(ClaimTypes.Name,user.Username),
                     new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };
            List<string> RoleIds = new List<string>();
            List<RolesModel> Roles = unitOfWork.RolesRepository.GetAll();
            foreach (var item in unitOfWork.UsertoRoleRepository.GetRolesForUser(user.ConcurrencyStamp))
            {
                RoleIds.Add(Roles.FirstOrDefault(u => u.ConcurrencyStamp == item).Name);
            }

            foreach (var userRole in RoleIds)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                User = user.Username,
                Roles = JsonSerializer.Serialize(RoleIds)
            });
        }

        private bool AddRoleToUser(string role, UsersModel user)
        {
            bool isok = false;
            bool rolenewadded = false;
            string RoleGuid = "";
            var dbRole = unitOfWork.RolesRepository.FindRoleByName(role);
            if (dbRole == null)
            {
                RoleGuid = Guid.NewGuid().ToString();
                unitOfWork.RolesRepository.Add(new RolesModel { Name = role, NormalizedName = role.ToUpper(), ConcurrencyStamp = RoleGuid });
                rolenewadded = true;
            }

            if (rolenewadded)
                unitOfWork.UsertoRoleRepository.Add(new UsertoRoleModel { UserID = user.ConcurrencyStamp, RoleID = RoleGuid });
            else
            {
                unitOfWork.UsertoRoleRepository.Add(new UsertoRoleModel { UserID = user.ConcurrencyStamp, RoleID = dbRole.ConcurrencyStamp });
            }
            isok = true;
            return isok;
        }

        private bool CheckPassword(UsersModel user, string password)
        {
            return securityutils.AreEqual(password, user.PasswordHash, unitOfWork.UsertoSaltRepository.GetSaltByGuid(user.ConcurrencyStamp));
        }

    }
}
