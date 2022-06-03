using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Authentication;

namespace PatientCareAPI.DataAccess.Repositories.Abstract
{
    public interface IRoletoYetkiRepository : IRepository<RoletoYetki>
    {
        void AddYetkitoRole(RoletoYetki model);

        void DeleteYetkisbyRole(string RoleGuid);

        List<string> GetYetkisByRole(string RoleId);
    }
}
