using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Authentication;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Auth
{
    public interface IUsertoSaltRepository : IRepository<UsertoSaltModel>
    {
        string GetSaltByGuid(string UserGuid);
    }
}
