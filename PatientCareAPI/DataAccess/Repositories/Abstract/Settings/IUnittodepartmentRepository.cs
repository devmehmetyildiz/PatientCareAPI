using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Settings
{
    public interface IUnittodepartmentRepository : IRepository<UnittoDepartmentModel>
    {
        void AddDepartments(List<DepartmentModel> List, string UnitGuid);

        void DeleteDepartmentsByUnit(string UnitGuid);
        void RemoveDepartmentsfromUnit(string UnitId);
    }
}
