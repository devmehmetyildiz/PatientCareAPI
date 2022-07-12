using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Authentication;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Auth
{
    public interface IAuthoryRepository : IRepository<AuthoryModel>
    {
        AuthoryModel FindAuthoryByName(string yetkiName);

        AuthoryModel FindAuthoryBuGuid(string Guid);

        List<AuthoryModel> GetAuthoriesbyGuids(List<string> authoryguids);
    }
}
