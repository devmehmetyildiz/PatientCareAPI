
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
using PatientCareAPI.Models.Global;
using PatientCareAPI.Models.Settings;

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
        private string GetSessionUser()
        {
            return (this.User.Identity as ClaimsIdentity).FindFirst(ClaimTypes.Name)?.Value;
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
            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                IsEssential = true
            };
            var tokenresponse = new JwtSecurityTokenHandler().WriteToken(token);
            HttpContext.Response.Cookies.Append(
                      "patientcare", tokenresponse,
                     cookieOptions);
            return Ok(new
            {
                token = tokenresponse,
                expiration = token.ValidTo,
                User = user.Username
            });
        }

        [Authorize]
        [HttpGet]
        [Route("GetActiveUser")]
        public async Task<IActionResult> GetActiveUser()
        {
            return Ok(GetSessionUser());
        }

        [Authorize]
        [HttpGet]
        [Route("GetUserMeta")]
        public async Task<IActionResult> GetUserMeta()
        {
            var Data = unitOfWork.UsersRepository.GetAll().FirstOrDefault(u => u.IsActive && u.Username == GetSessionUser());
            List<string> stations = unitOfWork.UsertoStationRepository.GetRecords<UsertoStationsModel>(u => u.UserID == Data.ConcurrencyStamp).Select(u => u.StationID).ToList();
            Data.Stations.AddRange(unitOfWork.StationsRepository.GetStationsbyGuids(stations));
            List<string> departments = unitOfWork.UsertoDepartmentRepository.GetAll().Where(u => u.UserID == Data.ConcurrencyStamp).Select(u => u.DepartmanID).ToList();
            Data.Departments.AddRange(unitOfWork.DepartmentRepository.GetDepartmentsbyGuids(departments));
            List<string> roles = unitOfWork.UsertoRoleRepository.GetRecords<UsertoRoleModel>(u => u.UserID == Data.ConcurrencyStamp).Select(u => u.RoleID).ToList();
            Data.Roles.AddRange(unitOfWork.RoleRepository.GetRolesbyGuids(roles));
            Data.Files = unitOfWork.FileRepository.GetRecords<FileModel>(u => u.Parentid == Data.ConcurrencyStamp);
            return Ok(Data);
        }

        [Authorize]
        [HttpGet]
        [Route("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles()
        {
            var userId = GetSessionUser();
            UsersModel User = unitOfWork.UsersRepository.GetRecord<UsersModel>(u => u.Username == userId);
            List<string> userroles = unitOfWork.UsertoRoleRepository.GetRecords<UsertoRoleModel>(u => u.UserID == User.ConcurrencyStamp).Select(u=>u.RoleID).ToList();
            List<string> roles = unitOfWork.RoleRepository.GetRolesbyGuids(userroles).Select(u=>u.Name).ToList();
            return Ok(roles);
        }

        [Authorize]
        [HttpGet]
        [Route("GetTableMeta")]
        public async Task<IActionResult> GetTableMeta()
        {
            return Ok(unitOfWork.TablemetaconfigRepository.GetRecords<TablemetaconfigModel>(u=>u.Username == GetSessionUser()));
        }

        [Authorize]
        [HttpPost]
        [Route("SaveTableMeta")]
        public async Task<IActionResult> SaveTableMeta(TablemetaconfigModel model)
        {
            var data = unitOfWork.TablemetaconfigRepository.GetRecord<TablemetaconfigModel>(u => u.Username == model.Username && u.Meta == model.Meta);
            if (data == null)
            {
                model.Username = GetSessionUser();
                unitOfWork.TablemetaconfigRepository.Add(model);
            }
            else
            {
                unitOfWork.TablemetaconfigRepository.update(unitOfWork.TablemetaconfigRepository.Getbyid(model.Id), model);
            }
            unitOfWork.Complate();
            return Ok(unitOfWork.TablemetaconfigRepository.GetRecords<TablemetaconfigModel>(u => u.Username == GetSessionUser()));
        }

        public async Task<IActionResult> CheckCredentials(LoginModel model)
        {
           
            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(PasswordModel model)
        {
            var user = unitOfWork.UsersRepository.FindUserByName(model.Username);
            if (!CheckPassword(user, model.Oldpassword))
            {
                return new ObjectResult(new ResponseModel { Status = "UnAuthorized Request", Massage = "Güncel Parola hatalı" }) { StatusCode = 403 };
            }
            UsersModel User = unitOfWork.UsersRepository.GetRecord<UsersModel>(u => u.Username == model.Username);
            var salt = securityutils.CreateSalt(30);
            unitOfWork.UsertoSaltRepository.Remove(unitOfWork.UsertoSaltRepository.GetRecord<UsertoSaltModel>(u => u.UserID == User.ConcurrencyStamp).Id);
            unitOfWork.UsertoSaltRepository.Add(new UsertoSaltModel { Salt = salt, UserID = User.ConcurrencyStamp });
            User.PasswordHash = securityutils.GenerateHash(model.Newpassword, salt);
            unitOfWork.UsersRepository.update(unitOfWork.UsersRepository.Getbyid(user.Id), user);
            unitOfWork.Complate();
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("CreateResetPasswordRequest")]
        public async Task<IActionResult> CreateResetPasswordRequest(string email)
        {
            //var user = unitOfWork.UsersRepository.GetRecord<UsersModel>(u => u.Username == username);
            //if (user == null)
            //{
            //    return new ObjectResult(new ResponseModel { Status = "UnAuthorized Request", Massage = "Kullanıcı Adı Bulunamadı" }) { StatusCode = 403 };
            //}

            //unitOfWork.ResetpasswordrequestRepository.GetRecords

            //var resetpasswordlist = unitOfWork.ResetpasswordrequestRepository.GetRecords<ResetpasswordrequestModel>(u =>
            // u.IsActive &&
            // u.Username == model.Username &&
            // u.Hashkey == model.Hashkey);
            //if (resetpasswordlist == null || resetpasswordlist.Where(u => u.Expiretime > DateTime.Now).ToList().Count == 0)
            //{
            //    return new ObjectResult(new ResponseModel { Status = "UnAuthorized Request", Massage = "Parola sıfırlama talebi bunumadı" }) { StatusCode = 403 };
            //}
           
            //unitOfWork.Complate();
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ChangePasswordUnauthorized")]
        public async Task<IActionResult> ChangePasswordUnauthorized(ResetpasswordrequestModel model)
        {
            var resetpasswordlist= unitOfWork.ResetpasswordrequestRepository.GetRecords<ResetpasswordrequestModel>(u =>
            u.IsActive &&
            u.Username == model.Username &&
            u.Hashkey == model.Hashkey);
            if(resetpasswordlist == null || resetpasswordlist.Where(u => u.Expiretime > DateTime.Now).ToList().Count==0)
            {
                return new ObjectResult(new ResponseModel { Status = "UnAuthorized Request", Massage = "Parola sıfırlama talebi bunumadı" }) { StatusCode = 403 };
            }
            var user = unitOfWork.UsersRepository.FindUserByName(model.Username);
            UsersModel User = unitOfWork.UsersRepository.GetRecord<UsersModel>(u => u.Username == model.Username);
            var salt = securityutils.CreateSalt(30);
            unitOfWork.UsertoSaltRepository.Remove(unitOfWork.UsertoSaltRepository.GetRecord<UsertoSaltModel>(u => u.UserID == User.ConcurrencyStamp).Id);
            unitOfWork.UsertoSaltRepository.Add(new UsertoSaltModel { Salt = salt, UserID = User.ConcurrencyStamp });
            User.PasswordHash = securityutils.GenerateHash(model.Newpassword, salt);
            unitOfWork.UsersRepository.update(unitOfWork.UsersRepository.Getbyid(user.Id), user);
            model.IsActive = false;
            unitOfWork.ResetpasswordrequestRepository.update(unitOfWork.ResetpasswordrequestRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetChangePasswordUnauthorized")]
        public async Task<IActionResult> GetChangePasswordUnauthorized(string hashkey)
        {
            var resetpasswordlist = unitOfWork.ResetpasswordrequestRepository.GetRecords<ResetpasswordrequestModel>(u =>
             u.IsActive &&
             u.Hashkey == hashkey);
            if (resetpasswordlist == null || resetpasswordlist.Where(u => u.Expiretime > DateTime.Now).ToList().Count == 0)
            {
                return new ObjectResult(new ResponseModel { Status = "UnAuthorized Request", Massage = "Parola sıfırlama talebi bunumadı" }) { StatusCode = 403 };
            }
            return Ok(resetpasswordlist[0]);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("CreateChangePasswordUnauthorized")]
        public async Task<IActionResult> CreateChangePasswordUnauthorized(string hashkey)
        {
            var resetpasswordlist = unitOfWork.ResetpasswordrequestRepository.GetRecords<ResetpasswordrequestModel>(u =>
             u.IsActive &&
             u.Hashkey == hashkey);
            if (resetpasswordlist == null || resetpasswordlist.Where(u => u.Expiretime > DateTime.Now).ToList().Count == 0)
            {
                return new ObjectResult(new ResponseModel { Status = "UnAuthorized Request", Massage = "Parola sıfırlama talebi bunumadı" }) { StatusCode = 403 };
            }
            return Ok(resetpasswordlist[0]);
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
            Roles.Add(new AuthoryModel { Group = UserAuthory.AdminGroup, Name = UserAuthory.Admin });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Roles, Name = UserAuthory.Roles_Screen });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Roles, Name = UserAuthory.Roles_Getselected });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Roles, Name = UserAuthory.Roles_Add });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Roles, Name = UserAuthory.Roles_Edit });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Roles, Name = UserAuthory.Roles_Delete });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Roles, Name = UserAuthory.Roles_Getreport });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Roles, Name = UserAuthory.Roles_Columnchange });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Department, Name = UserAuthory.Department_Screen });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Department, Name = UserAuthory.Department_Getselected });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Department, Name = UserAuthory.Department_Add });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Department, Name = UserAuthory.Department_Edit });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Department, Name = UserAuthory.Department_Delete });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Department, Name = UserAuthory.Department_Getreport });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Department, Name = UserAuthory.Department_Columnchange });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Station, Name = UserAuthory.Station_Screen });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Station, Name = UserAuthory.Station_Getselected });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Station, Name = UserAuthory.Station_Add });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Station, Name = UserAuthory.Station_Edit });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Station, Name = UserAuthory.Station_Delete });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Station, Name = UserAuthory.Station_Getreport });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Station, Name = UserAuthory.Station_Columnchange });
            Roles.Add(new AuthoryModel { Group = UserAuthory.User, Name = UserAuthory.User_Screen });
            Roles.Add(new AuthoryModel { Group = UserAuthory.User, Name = UserAuthory.User_Getselected });
            Roles.Add(new AuthoryModel { Group = UserAuthory.User, Name = UserAuthory.User_Add });
            Roles.Add(new AuthoryModel { Group = UserAuthory.User, Name = UserAuthory.User_Edit });
            Roles.Add(new AuthoryModel { Group = UserAuthory.User, Name = UserAuthory.User_Delete });
            Roles.Add(new AuthoryModel { Group = UserAuthory.User, Name = UserAuthory.User_Getreport });
            Roles.Add(new AuthoryModel { Group = UserAuthory.User, Name = UserAuthory.User_Columnchange });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Case, Name = UserAuthory.Case_Screen });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Case, Name = UserAuthory.Case_Getselected });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Case, Name = UserAuthory.Case_Add });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Case, Name = UserAuthory.Case_Edit });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Case, Name = UserAuthory.Case_Delete });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Case, Name = UserAuthory.Case_Getreport });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Case, Name = UserAuthory.Case_Columnchange });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Unit, Name = UserAuthory.Unit_Screen });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Unit, Name = UserAuthory.Unit_Getselected });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Unit, Name = UserAuthory.Unit_Add });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Unit, Name = UserAuthory.Unit_Edit });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Unit, Name = UserAuthory.Unit_Delete });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Unit, Name = UserAuthory.Unit_Getreport });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Unit, Name = UserAuthory.Unit_Columnchange });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Stockdefine, Name = UserAuthory.Stockdefine_Screen });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Stockdefine, Name = UserAuthory.Stockdefine_Getselected });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Stockdefine, Name = UserAuthory.Stockdefine_Add });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Stockdefine, Name = UserAuthory.Stockdefine_Edit });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Stockdefine, Name = UserAuthory.Stockdefine_Delete });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Stockdefine, Name = UserAuthory.Stockdefine_Getreport });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Stockdefine, Name = UserAuthory.Stockdefine_Columnchange });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Files, Name = UserAuthory.Files_Screen });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Files, Name = UserAuthory.Files_Getselected });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Files, Name = UserAuthory.Files_Add });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Files, Name = UserAuthory.Files_Delete });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Files, Name = UserAuthory.Files_Getreport });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Files, Name = UserAuthory.Files_Columnchange });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Costumertype, Name = UserAuthory.Costumertype_Screen });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Costumertype, Name = UserAuthory.Costumertype_Getselected });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Costumertype, Name = UserAuthory.Costumertype_Add });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Costumertype, Name = UserAuthory.Costumertype_Edit });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Costumertype, Name = UserAuthory.Costumertype_Delete });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Costumertype, Name = UserAuthory.Costumertype_Getreport });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Costumertype, Name = UserAuthory.Costumertype_Columnchange });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patinettype, Name = UserAuthory.Patienttype_Screen });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patinettype, Name = UserAuthory.Patienttype_Getselected });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patinettype, Name = UserAuthory.Patienttype_Add });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patinettype, Name = UserAuthory.Patienttype_Edit });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patinettype, Name = UserAuthory.Patienttype_Delete });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patinettype, Name = UserAuthory.Patienttype_Getreport });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patinettype, Name = UserAuthory.Patienttype_Columnchange });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Tododefine, Name = UserAuthory.Tododefine_Screen });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Tododefine, Name = UserAuthory.Tododefine_Getselected });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Tododefine, Name = UserAuthory.Tododefine_Add });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Tododefine, Name = UserAuthory.Tododefine_Edit });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Tododefine, Name = UserAuthory.Tododefine_Delete });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Tododefine, Name = UserAuthory.Tododefine_Getreport });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Tododefine, Name = UserAuthory.Tododefine_Columnchange });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Todogroupdefine, Name = UserAuthory.Todogroupdefine_Screen });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Todogroupdefine, Name = UserAuthory.Todogroupdefine_Getselected });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Todogroupdefine, Name = UserAuthory.Todogroupdefine_Add });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Todogroupdefine, Name = UserAuthory.Todogroupdefine_Edit });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Todogroupdefine, Name = UserAuthory.Todogroupdefine_Delete });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Todogroupdefine, Name = UserAuthory.Todogroupdefine_Getreport });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Todogroupdefine, Name = UserAuthory.Todogroupdefine_Columnchange });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Checkperiod, Name = UserAuthory.Checkperiod_Screen });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Checkperiod, Name = UserAuthory.Checkperiod_Getselected });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Checkperiod, Name = UserAuthory.Checkperiod_Add });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Checkperiod, Name = UserAuthory.Checkperiod_Edit });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Checkperiod, Name = UserAuthory.Checkperiod_Delete });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Checkperiod, Name = UserAuthory.Checkperiod_Getreport });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Checkperiod, Name = UserAuthory.Checkperiod_Columnchange });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Period, Name = UserAuthory.Period_Screen });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Period, Name = UserAuthory.Period_Getselected });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Period, Name = UserAuthory.Period_Add });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Period, Name = UserAuthory.Period_Edit });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Period, Name = UserAuthory.Period_Delete });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Period, Name = UserAuthory.Period_Getreport });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Period, Name = UserAuthory.Period_Columnchange });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Mailsetting, Name = UserAuthory.Mailsetting_Screen });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Mailsetting, Name = UserAuthory.Mailsetting_Getselected });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Mailsetting, Name = UserAuthory.Mailsetting_Add });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Mailsetting, Name = UserAuthory.Mailsetting_Edit });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Mailsetting, Name = UserAuthory.Mailsetting_Delete });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Mailsetting, Name = UserAuthory.Mailsetting_Getreport });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Mailsetting, Name = UserAuthory.Mailsetting_Columnchange });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Printtemplate, Name = UserAuthory.Printtemplate_Screen });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Printtemplate, Name = UserAuthory.Printtemplate_Getselected });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Printtemplate, Name = UserAuthory.Printtemplate_Add });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Printtemplate, Name = UserAuthory.Printtemplate_Edit });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Printtemplate, Name = UserAuthory.Printtemplate_Delete });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Printtemplate, Name = UserAuthory.Printtemplate_Getreport });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Printtemplate, Name = UserAuthory.Printtemplate_Columnchange });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Warehouse, Name = UserAuthory.Warehouse_Screen });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Warehouse, Name = UserAuthory.Warehouse_Getselected });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Warehouse, Name = UserAuthory.Warehouse_Add });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Warehouse, Name = UserAuthory.Warehouse_Edit });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Warehouse, Name = UserAuthory.Warehouse_Delete });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Warehouse, Name = UserAuthory.Warehouse_Getreport });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Warehouse, Name = UserAuthory.Warehouse_Columnchange });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Stock, Name = UserAuthory.Stock_Screen });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Stock, Name = UserAuthory.Stock_Getselected });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Stock, Name = UserAuthory.Stock_Add });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Stock, Name = UserAuthory.Stock_Edit });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Stock, Name = UserAuthory.Stock_Delete });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Stock, Name = UserAuthory.Stock_Getreport });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Stock, Name = UserAuthory.Stock_Columnchange });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Stockmovement, Name = UserAuthory.Stockmovement_Screen });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Stockmovement, Name = UserAuthory.Stockmovement_Getselected });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Stockmovement, Name = UserAuthory.Stockmovement_Add });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Stockmovement, Name = UserAuthory.Stockmovement_Edit });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Stockmovement, Name = UserAuthory.Stockmovement_Delete });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Stockmovement, Name = UserAuthory.Stockmovement_Getreport });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Stockmovement, Name = UserAuthory.Stockmovement_Columnchange });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Purchaseorder, Name = UserAuthory.Purchaseorder_Screen });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Purchaseorder, Name = UserAuthory.Purchaseorder_Getselected });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Purchaseorder, Name = UserAuthory.Purchaseorder_Add });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Purchaseorder, Name = UserAuthory.Purchaseorder_Edit });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Purchaseorder, Name = UserAuthory.Purchaseorder_Delete });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Purchaseorder, Name = UserAuthory.Purchaseorder_Getreport });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Purchaseorder, Name = UserAuthory.Purchaseorder_Columnchange });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Purchaseorderstock, Name = UserAuthory.Purchaseorderstock_Screen });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Purchaseorderstock, Name = UserAuthory.Purchaseorderstock_Getselected });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Purchaseorderstock, Name = UserAuthory.Purchaseorderstock_Add });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Purchaseorderstock, Name = UserAuthory.Purchaseorderstock_Edit });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Purchaseorderstock, Name = UserAuthory.Purchaseorderstock_Delete });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Purchaseorderstock, Name = UserAuthory.Purchaseorderstock_Getreport });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Purchaseorderstock, Name = UserAuthory.Purchaseorderstock_Columnchange });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Purchaseorderstockmovement, Name = UserAuthory.Purchaseorderstockmovement_Screen });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Purchaseorderstockmovement, Name = UserAuthory.Purchaseorderstockmovement_Getselected });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Purchaseorderstockmovement, Name = UserAuthory.Purchaseorderstockmovement_Add });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Purchaseorderstockmovement, Name = UserAuthory.Purchaseorderstockmovement_Edit });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Purchaseorderstockmovement, Name = UserAuthory.Purchaseorderstockmovement_Delete });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Purchaseorderstockmovement, Name = UserAuthory.Purchaseorderstockmovement_Getreport });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Purchaseorderstockmovement, Name = UserAuthory.Purchaseorderstockmovement_Columnchange });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Preregistration, Name = UserAuthory.Preregistration_Screen });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Preregistration, Name = UserAuthory.Preregistration_Getselected });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Preregistration, Name = UserAuthory.Preregistration_Add });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Preregistration, Name = UserAuthory.Preregistration_Edit });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Preregistration, Name = UserAuthory.Preregistration_Delete });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Preregistration, Name = UserAuthory.Preregistration_Getreport });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Preregistration, Name = UserAuthory.Preregistration_Columnchange });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patients, Name = UserAuthory.Patients_Screen });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patients, Name = UserAuthory.Patients_Getselected });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patients, Name = UserAuthory.Patients_Add });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patients, Name = UserAuthory.Patients_Edit });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patients, Name = UserAuthory.Patients_Delete });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patients, Name = UserAuthory.Patients_Getreport });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patients, Name = UserAuthory.Patients_Columnchange });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patientstock, Name = UserAuthory.Patientstock_Screen });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patientstock, Name = UserAuthory.Patientstock_Getselected });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patientstock, Name = UserAuthory.Patientstock_Add });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patientstock, Name = UserAuthory.Patientstock_Edit });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patientstock, Name = UserAuthory.Patientstock_Delete });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patientstock, Name = UserAuthory.Patientstock_Getreport });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patientstock, Name = UserAuthory.Patientstock_Columnchange });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patientstockmovement, Name = UserAuthory.Patientstockmovement_Screen });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patientstockmovement, Name = UserAuthory.Patientstockmovement_Getselected });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patientstockmovement, Name = UserAuthory.Patientstockmovement_Add });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patientstockmovement, Name = UserAuthory.Patientstockmovement_Edit });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patientstockmovement, Name = UserAuthory.Patientstockmovement_Delete });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patientstockmovement, Name = UserAuthory.Patientstockmovement_Getreport });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patientstockmovement, Name = UserAuthory.Patientstockmovement_Columnchange });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patientdefine, Name = UserAuthory.Patientdefine_Screen });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patientdefine, Name = UserAuthory.Patientdefine_Getselected });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patientdefine, Name = UserAuthory.Patientdefine_Add });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patientdefine, Name = UserAuthory.Patientdefine_Edit });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patientdefine, Name = UserAuthory.Patientdefine_Delete });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patientdefine, Name = UserAuthory.Patientdefine_Getreport });
            Roles.Add(new AuthoryModel { Group = UserAuthory.Patientdefine, Name = UserAuthory.Patientdefine_Columnchange });
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
