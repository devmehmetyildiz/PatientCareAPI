using PatientCareAPI.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Auth
{
    public interface IRoleRepository : IRepository<RoleModel>
    {
        RoleModel FindByName(string name);
    }
}
