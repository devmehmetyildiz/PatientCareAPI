using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Settings
{
    public interface ICostumertypetoDepartmentRepository : IRepository<CostumertypetoDepartmentModel>
    {
        void AddDepartments(List<DepartmentModel> List, string CostumertypeGuid);

        void DeleteDepartmentsByCostumertype(string CostumertypeGuid);
    }
}