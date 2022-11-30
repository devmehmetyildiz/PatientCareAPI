
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

namespace PatientCareAPI.Controllers.Auth
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
        [HttpGet]
        [Route("Test")]
        public IActionResult Test()
        {
            return Ok("OK");
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("DBTest")]
        public IActionResult DBTest()
        {
            return Ok($"Aktif Kullanıcı Sayısı = {unitOfWork.UsersRepository.GetAll().Count}");
        }

        //Admin Kullanıcının Oluşturulması
        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            List<ResponseModel> Notification = new List<ResponseModel>();
            if (unitOfWork.UsersRepository.GetAll().Count>0)
            {
                Notification.Add(new ResponseModel { Status = "ERROR", Massage = "Admin Kullanıcı Oluşturuldu. Bu fonksiyon geçersiz" });
                return BadRequest(Notification);
            } 
            var userExist = unitOfWork.UsersRepository.FindUserByName(model.Username);
            if (userExist != null)
            {
                Notification.Add(new ResponseModel { Status = "Error", Massage = "Bu Kullanıcı Adı Daha Önce Alındı" });
                return StatusCode(StatusCodes.Status500InternalServerError, Notification);
            }
            var userGuid = Guid.NewGuid().ToString();
            var RoleGuid = Guid.NewGuid().ToString();
            var salt = securityutils.CreateSalt(30);
            unitOfWork.UsertoSaltRepository.Add(new UsertoSaltModel { Salt = salt, UserID = userGuid });
            UsersModel user = new UsersModel()
            {
                Id = 0,
                Username = model.Username,
                NormalizedUsername = model.Username.ToUpper(),
                ConcurrencyStamp = userGuid,
                Email = model.Email,
                AccessFailedCount = 0,
                IsActive = true,
                CreatedUser = "System",
                CreateTime = DateTime.Now,
                PasswordHash = securityutils.GenerateHash(model.Password, salt),
                PhoneNumber = "",
            };
            unitOfWork.UsersRepository.Add(user);
            ConfigureRoles();
            unitOfWork.RoleRepository.Add(new RoleModel { Id = 0, ConcurrencyStamp = RoleGuid, CreatedUser = "System", CreateTime = DateTime.Now, IsActive = true, Name = "Admin" });
            unitOfWork.RoletoAuthoryRepository.AddAuthorytoRole(new RoletoAuthory { Id = 0, RoleID = RoleGuid, AuthoryID = unitOfWork.AuthoryRepository.FindAuthoryByName(UserAuthory.Admin).ConcurrencyStamp });
            unitOfWork.UsertoRoleRepository.AddRolestoUser(new UsertoRoleModel { Id = 0, RoleID = RoleGuid, UserID = userGuid });
            unitOfWork.Complate();
            Notification.Add(new ResponseModel { Status = "Success", Massage = "Kullanıcı Başarı ile Oluşturuldu" });
            return Ok(Notification);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            List<ResponseModel> Notification = new List<ResponseModel>();

            var user = unitOfWork.UsersRepository.FindUserByName(model.Username);
            if ((user == null))
            {
                Notification.Add(new ResponseModel { Status = "Error", Massage = "Kullanıcı Bulunamadı" });
                return NotFound(Notification);
            }
            if (!CheckPassword(user, model.Password))
            {
                Notification.Add(new ResponseModel { Status = "Error", Massage = "Kullanıcı Adı veya Şifre Hatalı" });
                return Unauthorized(Notification);
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

        [Authorize]
        [HttpGet]
        [Route("GetActiveUser")]
        public async Task<IActionResult> GetActiveUser()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            return Ok(userId);
        }

        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [HttpGet]
        [Route("ConfigureRoles")]
        public async Task<IActionResult> ConfigureRolesAsAdmin()
        {
            ConfigureRoles();
            return Ok();
        }

        private bool CheckPassword(UsersModel user, string password)
        {
            return securityutils.AreEqual(password, user.PasswordHash, unitOfWork.UsertoSaltRepository.GetSaltByGuid(user.ConcurrencyStamp));
        }

        private async void ConfigureRoles()
        {
            List<AuthoryModel> Roles = new List<AuthoryModel>();
            List<AuthoryModel> newRoles = new List<AuthoryModel>();
            Roles.Add(new AuthoryModel{ Group=UserAuthory.BaseGroup,Name=UserAuthory.Basic });
            Roles.Add(new AuthoryModel{ Group=UserAuthory.BaseGroup,Name=UserAuthory.Admin });
            Roles.Add(new AuthoryModel{ Group=UserAuthory.User,Name=UserAuthory.User_Screen });
            Roles.Add(new AuthoryModel{ Group=UserAuthory.User,Name=UserAuthory.User_Add });
            Roles.Add(new AuthoryModel{ Group=UserAuthory.User,Name=UserAuthory.User_Update });
            Roles.Add(new AuthoryModel{ Group=UserAuthory.User,Name=UserAuthory.User_Delete });
            Roles.Add(new AuthoryModel{ Group=UserAuthory.User,Name=UserAuthory.User_ManageAll});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Department,Name=UserAuthory.Department_Screen});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Department,Name=UserAuthory.Department_Add});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Department,Name=UserAuthory.Department_Update});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Department,Name=UserAuthory.Department_Delete});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Department,Name=UserAuthory.Department_ManageAll});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Stock,Name=UserAuthory.Stock_Screen});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Stock,Name=UserAuthory.Stock_Add});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Stock,Name=UserAuthory.Stock_Update});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Stock,Name=UserAuthory.Stock_Delete});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Stock,Name=UserAuthory.Stock_ManageAll});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Process,Name=UserAuthory.Process_Screen});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Process,Name=UserAuthory.Process_Add});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Process,Name=UserAuthory.Process_Update});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Process,Name=UserAuthory.Process_Delete});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Process,Name=UserAuthory.Process_ManageAll});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Patients,Name=UserAuthory.Patients_Screen});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Patients,Name=UserAuthory.Patients_Add});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Patients,Name=UserAuthory.Patients_Update});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Patients,Name=UserAuthory.Patients_Delete});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Patients,Name=UserAuthory.Patients_ManageAll});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Patients,Name=UserAuthory.Patients_UploadFile});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Patients,Name=UserAuthory.Patients_DownloadFile});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Patients,Name=UserAuthory.Patients_ViewFile});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Patienttype,Name=UserAuthory.Patienttype_Screen});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Patienttype,Name=UserAuthory.Patienttype_Add});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Patienttype,Name=UserAuthory.Patienttype_Update});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Patienttype,Name=UserAuthory.Patienttype_Delete});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Patienttype,Name=UserAuthory.Patienttype_ManageAll});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Unit,Name=UserAuthory.Unit_Screen});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Unit,Name=UserAuthory.Unit_Add});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Unit,Name=UserAuthory.Unit_Update});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Unit,Name=UserAuthory.Unit_Delete});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Unit,Name=UserAuthory.Unit_ManageAll});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Stations,Name=UserAuthory.Stations_Screen});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Stations,Name=UserAuthory.Stations_Add});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Stations,Name=UserAuthory.Stations_Update});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Stations,Name=UserAuthory.Stations_Delete});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Stations,Name=UserAuthory.Stations_ManageAll});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Case,Name=UserAuthory.Case_Screen});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Case,Name=UserAuthory.Case_Add});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Case,Name=UserAuthory.Case_Update});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Case,Name=UserAuthory.Case_Delete});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Case,Name=UserAuthory.Case_ManageAll});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Costumertype,Name=UserAuthory.Costumertype_Screen });
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Costumertype,Name=UserAuthory.Costumertype_Add });
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Costumertype,Name=UserAuthory.Costumertype_Update });
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Costumertype,Name=UserAuthory.Costumertype_Delete });
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Costumertype,Name=UserAuthory.Costumertype_ManageAll });
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Roles,Name=UserAuthory.Roles_Screen});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Roles,Name=UserAuthory.Roles_Add});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Roles,Name=UserAuthory.Roles_Update});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Roles,Name=UserAuthory.Roles_Delete});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Roles,Name=UserAuthory.Roles_ManageAll});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.File,Name=UserAuthory.File_Screen});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.File,Name=UserAuthory.File_Add});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.File,Name=UserAuthory.File_Update});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.File,Name=UserAuthory.File_Delete});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.File,Name=UserAuthory.File_ManageAll});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Dashboard,Name=UserAuthory.Dashboard_AllScreen});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Dashboard,Name=UserAuthory.Dashboard_DepartmentScreen});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Reminding,Name=UserAuthory.Reminding_Screen});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Reminding,Name=UserAuthory.Reminding_Add});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Reminding,Name=UserAuthory.Reminding_Update});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Reminding,Name=UserAuthory.Reminding_Delete});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Reminding,Name=UserAuthory.Reminding_ManageAll});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Reminding,Name=UserAuthory.Reminding_DefineforAll});
            Roles.Add(new AuthoryModel{ Group=UserAuthory.Reminding,Name=UserAuthory.Reminding_Define});
            foreach (var role in Roles)
            {
                var dbRole = unitOfWork.AuthoryRepository.FindAuthoryByName(role.Name);
                if (dbRole == null)
                {
                    var model = new AuthoryModel { Name = role.Name, Group = role.Group, ConcurrencyStamp = Guid.NewGuid().ToString() };
                    unitOfWork.AuthoryRepository.Add(model);
                    newRoles.Add(model);
                }
            }
            if (newRoles.Count > 0)
            {
                unitOfWork.Complate();
            }
        }

    }
}
