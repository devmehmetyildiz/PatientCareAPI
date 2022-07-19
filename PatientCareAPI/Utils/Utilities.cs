using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using PatientCareAPI.DataAccess;
using PatientCareAPI.Models.Authentication;

namespace PatientCareAPI.Utils
{
    public class Utilities
    {
        UnitOfWork unitOfWork;
        private readonly ApplicationDBContext _context;

        public Utilities(ApplicationDBContext context)
        {
            _context = context;
            unitOfWork = new UnitOfWork(_context);
        }
        private List<string> GetUserRoles(object userid)
        {
            var claimsIdentity = userid as ClaimsIdentity;
            var user = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            UsersModel Activeuser = unitOfWork.UsersRepository.FindUserByName(user.ToString());
            List<string> Userroles = unitOfWork.UsertoRoleRepository.GetRolesbyUser(Activeuser.ConcurrencyStamp);
            List<string> Userauthories = new List<string>();
            foreach (var roles in Userroles)
            {
                foreach (var authories in unitOfWork.RoletoAuthoryRepository.GetAuthoriesByRole(roles))
                {
                    Userauthories.Add(unitOfWork.AuthoryRepository.FindAuthoryBuGuid(authories).Name);
                }
            }
            return Userauthories;
        }

        public bool CheckAuth(string Role,object userid)
        {
            var claimsIdentity = userid as ClaimsIdentity;
            var user = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            bool isOk = unitOfWork.AuthoryRepository.CheckAuthByUsername(user, Role);
            //if (GetUserRoles(userid).Contains(Role))
            //{
            //    isOk = true;
            //}
            return isOk;
        }
    }
}
