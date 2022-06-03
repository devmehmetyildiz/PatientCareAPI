using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Authentication;

namespace PatientCareAPI.DataAccess.Repositories.Abstract
{
    public interface IRoletoAuthoryRepository : IRepository<RoletoAuthory>
    {
        void AddAuthorytoRole(RoletoAuthory model);

        void DeleteAuthoriesbyRole(string RoleGuid);

        List<string> GetAuthoriesByRole(string RoleId);
    }
}
