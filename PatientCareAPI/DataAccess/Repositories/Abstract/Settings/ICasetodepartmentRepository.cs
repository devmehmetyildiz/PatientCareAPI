using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Settings
{
    public interface ICasetodepartmentRepository : IRepository<CasetoDepartmentModel>
    {
        void AddDepartments(List<DepartmentModel> List, string CaseGuid);

        void DeleteDepartmentsByCase(string CaseGuid);
    }
}
