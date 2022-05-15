using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Authentication;

namespace PatientCareAPI.DataAccess.Repositories.Abstract
{
    public interface IUsertoAuthoryRepository : IRepository<UsertoAuthoryModel>
    {
        void AddAuthtoUser(UsertoAuthoryModel model);

        List<string> GetAuthsbyUser(string UserID);
    }
}
