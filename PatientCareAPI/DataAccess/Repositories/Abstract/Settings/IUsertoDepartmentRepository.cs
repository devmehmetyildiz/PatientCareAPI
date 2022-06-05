using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Settings
{
    public interface IUsertoDepartmentRepository : IRepository<UsertoDepartmentModel>
    {
        List<string> GetDepartmentsbyUser(string UserID);
        void RemoveDepartmentfromUser(string UserID);
        void AddDepartmenttoUser(UsertoDepartmentModel model);
    }
}
