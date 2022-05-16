using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Authentication;

namespace PatientCareAPI.DataAccess.Repositories.Abstract
{
    public interface IAuthorytoRolesRepository : IRepository<AuthorytoRoles>
    {
        void AddRoletoAuth(AuthorytoRoles model);

        void DeleteRolesbyAuth(string AuthGuid);

        List<string> GetRolesByAuth(string AuthId);
    }
}
