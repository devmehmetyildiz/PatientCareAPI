using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PatientCareAPI.DataAccess;
using PatientCareAPI.Models.Authentication;
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

        public UsersController(IConfiguration configuration, ILogger<UsersController> logger, ApplicationDBContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
            unitOfWork = new UnitOfWork(context);
        }

        [Authorize(Roles = UserAuthory.User_Screen)]
        [Route("GetAll")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var Users = unitOfWork.UsersRepository.GetAll().Where(u => u.IsActive).ToList();
            var Departments = unitOfWork.DepartmentRepository.GetAll().Where(u => u.IsActive).ToList();
            var Stations = unitOfWork.StationsRepository.GetAll().Where(u => u.IsActive).ToList();
            var Roles = unitOfWork.RoleRepository.GetAll().Where(u => u.IsActive).ToList();
            foreach (var user in Users)
            {
                var userstations = unitOfWork.UsertoStationRepository.GetStationsbyUser(user.ConcurrencyStamp);
                var userdepartments = unitOfWork.UsertoDepartmentRepository.GetDepartmentsbyUser(user.ConcurrencyStamp);
                var userroles = unitOfWork.UsertoRoleRepository.GetRolesbyUser(user.ConcurrencyStamp);
                foreach (var item in userstations)
                {
                    user.Stations.Add(Stations.FirstOrDefault(u => u.ConcurrencyStamp == item));
                }
                foreach (var item in userdepartments)
                {
                    user.Departments.Add(Departments.FirstOrDefault(u => u.ConcurrencyStamp == item));
                }
                foreach (var item in userroles)
                {
                    user.Roles.Add(Roles.FirstOrDefault(u => u.ConcurrencyStamp == item));
                }
            }
            return Ok(Users);
        }

        [Authorize(Roles = UserAuthory.User_Screen)]
        [Route("GetSelectedUser")]
        [HttpGet]
        public IActionResult GetSelectedUser(int ID)
        {
            var User = unitOfWork.UsersRepository.Getbyid(ID);
            var Departments = unitOfWork.DepartmentRepository.GetAll().Where(u => u.IsActive).ToList();
            var Stations = unitOfWork.StationsRepository.GetAll().Where(u => u.IsActive).ToList();
            var Roles = unitOfWork.RoleRepository.GetAll().Where(u => u.IsActive).ToList();
            var userstations = unitOfWork.UsertoStationRepository.GetStationsbyUser(User.ConcurrencyStamp);
            var userdepartments = unitOfWork.UsertoDepartmentRepository.GetDepartmentsbyUser(User.ConcurrencyStamp);
            var userroles = unitOfWork.UsertoRoleRepository.GetRolesbyUser(User.ConcurrencyStamp);
            foreach (var item in userstations)
            {
                User.Stations.Add(Stations.FirstOrDefault(u => u.ConcurrencyStamp == item));
            }
            foreach (var item in userdepartments)
            {
                User.Departments.Add(Departments.FirstOrDefault(u => u.ConcurrencyStamp == item));
            }
            foreach (var item in userroles)
            {
                User.Roles.Add(Roles.FirstOrDefault(u => u.ConcurrencyStamp == item));
            }
            return Ok(User);
        }


        [Authorize(Roles = UserAuthory.User_Add)]
        [Route("Add")]
        [HttpPost]
        public IActionResult Add(UsersModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var username = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            model.CreatedUser = username;
            model.IsActive = true;
            model.CreateTime = DateTime.Now;
            model.ConcurrencyStamp = Guid.NewGuid().ToString();
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

        [Authorize(Roles = (UserAuthory.User_Screen + "," + UserAuthory.User_Update))]
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

        [Authorize(Roles = UserAuthory.User_Delete)]
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

        [Authorize(Roles = UserAuthory.Admin)]
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
