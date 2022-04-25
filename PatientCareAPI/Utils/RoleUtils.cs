using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.DataAccess;
using PatientCareAPI.Models.Authentication;

namespace PatientCareAPI.Utils
{
    public class RoleUtils
    {
        UnitOfWork unitOfWork;
        ApplicationDBContext context;
        public RoleUtils()
        {         
            unitOfWork = new UnitOfWork(context);
        }

        public bool AddRoleToUser(string role,UsersModel user)
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
    }
}
