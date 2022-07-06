using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Authentication;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Auth
{
    public interface IUsersRepository : IRepository<UsersModel>
    {
        UsersModel FindUserByName(string name);

        UsersModel GetUsertByGuid(string guid);

        //UsersModel FindUserByPassword(string password);
    }
}
