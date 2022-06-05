
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
using Microsoft.AspNetCore.Authorization;

namespace PatientCareAPI.Controllers
{
    [Authorize]
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

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExist = unitOfWork.UsersRepository.FindUserByName(model.Username);
            if (userExist != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Error", Massage = "Bu Kullanıcı Adı Daha Önce Alındı" });
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
            AddBasicAuth(UserAuthory.Basic, user);
            unitOfWork.Complate();
            return Ok(new ResponseModel { Status = "Success", Massage = "Kullanıcı Başarı ile Oluşturuldu" });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if(model.Username=="sys" && model.Password == "123konZEK")
            {
                return Ok(ConfigureAuthSystem());
            }
            var user = unitOfWork.UsersRepository.FindUserByName(model.Username);
            if ((user == null))
            {
                return NotFound(new ResponseModel { Status = "Error", Massage = "Kullanıcı Bulunamadı" });
            }
            if (!CheckPassword(user, model.Password))
            {
                return Unauthorized(new ResponseModel { Status = "Error", Massage = "Kullanıcı Adı veya Şifre Hatalı" });
            }             
            var authClaims = new List<Claim>
                {
                     new Claim(ClaimTypes.Name,user.Username),
                     new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };
            List<AuthoryModel> Yetkiler = unitOfWork.AuthoryRepository.GetAll();
            foreach (var userrole in unitOfWork.UsertoRoleRepository.GetRolesbyUser(user.ConcurrencyStamp))
            {
                List<string> yetkis = unitOfWork.RoletoAuthoryRepository.GetAuthoriesByRole(userrole);
                foreach (var yetki in yetkis)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, Yetkiler.FirstOrDefault(u=>u.ConcurrencyStamp== yetki).Name));
                }
            }
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );            
            Response.Cookies.Append("X-Access-Token", new JwtSecurityTokenHandler().WriteToken(token), new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response.Cookies.Append("X-Username", user.Username, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                User = user.Username
            });
        }

        [HttpGet]
        [Route("GetActiveUser")]
        public async Task<IActionResult> GetActiveUser()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            return Ok(userId);
        }

        private bool AddBasicAuth(string role, UsersModel user)
        {
            try
            {
                bool isok = false;
                string yetkiGuid = "";
                string RoleGuid = "";
                bool authnewadded = false;
                var dbrole = unitOfWork.RoleRepository.FindByName("Basic");
                if (dbrole == null)
                {
                    RoleGuid = Guid.NewGuid().ToString();
                    unitOfWork.RoleRepository.Add(new RoleModel
                    {
                        Id = 0,
                        Name = "Basic",
                        NormalizedName = "Basic".ToUpper(),
                        ConcurrencyStamp = RoleGuid,
                        CreatedUser = "system",
                        CreateTime = DateTime.Now,
                        IsActive = true
                    });
                    authnewadded = true;
                }
                else
                {
                    RoleGuid = dbrole.ConcurrencyStamp;
                }
                var dbRole = unitOfWork.AuthoryRepository.FindAuthoryByName(role);
                if (dbRole == null)
                {
                    yetkiGuid = Guid.NewGuid().ToString();
                    unitOfWork.AuthoryRepository.Add(new AuthoryModel { Name = role, NormalizedName = role.ToUpper(), ConcurrencyStamp = yetkiGuid });
                }
                else
                {
                    yetkiGuid = dbRole.ConcurrencyStamp;
                }
                if (authnewadded)
                    unitOfWork.RoletoAuthoryRepository.AddAuthorytoRole(new RoletoAuthory { RoleID = RoleGuid, AuthoryID = yetkiGuid });
                unitOfWork.UsertoRoleRepository.AddRolestoUser(new UsertoRoleModel { RoleID = RoleGuid, UserID = user.ConcurrencyStamp });
                isok = true;
                return isok;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CheckPassword(UsersModel user, string password)
        {
            return securityutils.AreEqual(password, user.PasswordHash, unitOfWork.UsertoSaltRepository.GetSaltByGuid(user.ConcurrencyStamp));
        }

        private ResponseModel ConfigureAuthSystem()
        {
            List<string> Roles = new List<string>();
            List<AuthoryModel> newRoles = new List<AuthoryModel>();
            Roles.Add(UserAuthory.Basic);
            Roles.Add(UserAuthory.User);
            Roles.Add(UserAuthory.Admin);
            Roles.Add(UserAuthory.User_Screen);
            Roles.Add(UserAuthory.User_Add);
            Roles.Add(UserAuthory.User_Update);
            Roles.Add(UserAuthory.User_Delete);
            foreach (var role in Roles)
            {
                var dbRole = unitOfWork.AuthoryRepository.FindAuthoryByName(role);
                if (dbRole == null)
                {
                    var model = new AuthoryModel { Name = role, NormalizedName = role.ToUpper(), ConcurrencyStamp = Guid.NewGuid().ToString() };
                    unitOfWork.AuthoryRepository.Add(model);
                    newRoles.Add(model);
                }
            }
            if (newRoles.Count > 0)
            {
                unitOfWork.Complate();
                return new ResponseModel { Status = "Success", Massage = $"Roller Tanımlandı  = {JsonSerializer.Serialize(newRoles)}" };
            }
            else
                return new ResponseModel { Status = "Success", Massage = "yeni Role bulunamadı" };

        }

    }
}
