using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PatientCareAPI.DataAccess;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Models.Settings;
using PatientCareAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PatientCareAPI.Controllers.Auth
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<UsersController> _logger;
        private readonly ApplicationDBContext _context;
        UnitOfWork unitOfWork;
        CryptographyProcessor securityutils;
        public UsersController(IConfiguration configuration, ILogger<UsersController> logger, ApplicationDBContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
            unitOfWork = new UnitOfWork(context);
            securityutils = new CryptographyProcessor();
        }

        [AuthorizeMultiplePolicy(UserAuthory.User_Screen + "," + UserAuthory.Admin)]
        [Route("GetAll")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var Users = unitOfWork.UsersRepository.GetAll().Where(u => u.IsActive).ToList();
            foreach (var user in Users)
            {
                List<string> stations = unitOfWork.UsertoStationRepository.GetRecords<UsertoStationsModel>(u => u.UserID == user.ConcurrencyStamp).Select(u => u.StationID).ToList();
                user.Stations.AddRange(unitOfWork.StationsRepository.GetStationsbyGuids(stations));

                List<string> departments = unitOfWork.UsertoDepartmentRepository.GetAll().Where(u => u.UserID == user.ConcurrencyStamp).Select(u => u.DepartmanID).ToList();
                user.Departments.AddRange(unitOfWork.DepartmentRepository.GetDepartmentsbyGuids(departments));

                List<string> roles = unitOfWork.UsertoRoleRepository.GetRecords<UsertoRoleModel>(u => u.UserID == user.ConcurrencyStamp).Select(u => u.RoleID).ToList();
                user.Roles.AddRange(unitOfWork.RoleRepository.GetRolesbyGuids(roles));
            }
            return Ok(Users);
        }

        [AuthorizeMultiplePolicy(UserAuthory.User_Screen)]
        [Route("GetSelectedUser")]
        [HttpGet]
        public IActionResult GetSelectedUser(int ID)
        {
            var user = unitOfWork.UsersRepository.Getbyid(ID);
            List<string> stations = unitOfWork.UsertoStationRepository.GetAll().Where(u => u.UserID == user.ConcurrencyStamp).Select(u => u.StationID).ToList();
            user.Stations.AddRange(unitOfWork.StationsRepository.GetStationsbyGuids(stations));

            List<string> departments = unitOfWork.UsertoDepartmentRepository.GetAll().Where(u => u.UserID == user.ConcurrencyStamp).Select(u => u.DepartmanID).ToList();
            user.Departments.AddRange(unitOfWork.DepartmentRepository.GetDepartmentsbyGuids(departments));

            List<string> roles = unitOfWork.UsertoRoleRepository.GetAll().Where(u => u.UserID == user.ConcurrencyStamp).Select(u => u.RoleID).ToList();
            user.Roles.AddRange(unitOfWork.RoleRepository.GetRolesbyGuids(roles));
            return Ok(user);
        }

        [AuthorizeMultiplePolicy(UserAuthory.User_Add)]
        [Route("Add")]
        [HttpPost]
        public IActionResult Add(UsersModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.NormalizedUsername = model.Username.ToUpper();
            var userExist = unitOfWork.UsersRepository.FindUserByName(model.Username);
            if (userExist != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Error", Massage = "Bu Kullanıcı Adı Daha Önce Alındı" });
            var userGuid = Guid.NewGuid().ToString();
            var salt = securityutils.CreateSalt(30);
            unitOfWork.UsertoSaltRepository.Add(new UsertoSaltModel { Salt = salt, UserID = userGuid });
            model.PasswordHash = securityutils.GenerateHash(model.PasswordHash, salt);
            model.ConcurrencyStamp = userGuid;
            unitOfWork.UsersRepository.Add(model);
            foreach (var role in model.Roles)
            {
                unitOfWork.UsertoRoleRepository.AddRolestoUser(new UsertoRoleModel { RoleID = role.ConcurrencyStamp, UserID = model.ConcurrencyStamp });
            }
            foreach (var department in model.Departments)
            {
                unitOfWork.UsertoDepartmentRepository.AddDepartmenttoUser(new Models.Settings.UsertoDepartmentModel { DepartmanID = department.ConcurrencyStamp, UserID = model.ConcurrencyStamp });
            }
            foreach (var station in model.Stations)
            {
                unitOfWork.UsertoStationRepository.AddStationtoUser(new Models.Settings.UsertoStationsModel { StationID = station.ConcurrencyStamp, UserID = model.ConcurrencyStamp });
            }
            unitOfWork.Complate();
            return Ok();
        }

        [AuthorizeMultiplePolicy(UserAuthory.User_Screen + "," + UserAuthory.User_Update)]
        [Route("Update")]
        [HttpPost]
        public IActionResult Update(UsersModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.UpdatedUser = username;
            model.UpdateTime = DateTime.Now;
            unitOfWork.UsersRepository.update(unitOfWork.UsersRepository.Getbyid(model.Id), model);
            unitOfWork.UsertoRoleRepository.RemoveRolefromUser(model.ConcurrencyStamp);
            unitOfWork.UsertoDepartmentRepository.RemoveDepartmentfromUser(model.ConcurrencyStamp);
            unitOfWork.UsertoStationRepository.RemoveStationsfromUser(model.ConcurrencyStamp);
            foreach (var role in model.Roles)
            {
                unitOfWork.UsertoRoleRepository.AddRolestoUser(new UsertoRoleModel { RoleID = role.ConcurrencyStamp, UserID = model.ConcurrencyStamp });
            }
            foreach (var department in model.Departments)
            {
                unitOfWork.UsertoDepartmentRepository.AddDepartmenttoUser(new Models.Settings.UsertoDepartmentModel { DepartmanID = department.ConcurrencyStamp, UserID = model.ConcurrencyStamp });
            }
            foreach (var station in model.Stations)
            {
                unitOfWork.UsertoStationRepository.AddStationtoUser(new Models.Settings.UsertoStationsModel { StationID = station.ConcurrencyStamp, UserID = model.ConcurrencyStamp });
            }
            unitOfWork.Complate();
            return Ok();
        }

        [AuthorizeMultiplePolicy(UserAuthory.User_Delete)]
        [Route("Delete")]
        [HttpDelete]
        public IActionResult Delete(UsersModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.DeleteUser = username;
            model.IsActive = false;
            model.DeleteTime = DateTime.Now;
            unitOfWork.UsersRepository.update(unitOfWork.UsersRepository.Getbyid(model.Id), model);
            unitOfWork.Complate();
            return Ok();
        }

        [AuthorizeMultiplePolicy(UserAuthory.Admin)]
        [Route("DeleteFromDB")]
        [HttpDelete]
        public IActionResult DeleteFromDB(RoleModel model)
        {
            unitOfWork.UsersRepository.Remove(model.Id);
            unitOfWork.UsertoRoleRepository.RemoveRolefromUser(model.ConcurrencyStamp);
            unitOfWork.UsertoDepartmentRepository.RemoveDepartmentfromUser(model.ConcurrencyStamp);
            unitOfWork.UsertoStationRepository.RemoveStationsfromUser(model.ConcurrencyStamp);
            unitOfWork.Complate();
            return Ok();
        }
    }
}
