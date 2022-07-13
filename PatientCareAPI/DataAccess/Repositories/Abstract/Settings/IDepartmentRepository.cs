using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Settings
{
    public interface IDepartmentRepository : IRepository<DepartmentModel>
    {
        DepartmentModel GetDepartmentByGuid(string guid);
        List<DepartmentModel> GetDepartmentsbyGuids(List<string> departments);
    }
}
