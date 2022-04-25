using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Authentication;

namespace PatientCareAPI.DataAccess.Repositories.Abstract
{
    public interface IRolesRepository : IRepository<RolesModel>
    {
        RolesModel FindRoleByName(string RoleName);
        
    }
}
