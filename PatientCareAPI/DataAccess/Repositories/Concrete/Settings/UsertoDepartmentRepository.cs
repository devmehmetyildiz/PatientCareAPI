using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Settings;
using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Settings
{
    public class UsertoDepartmentRepository : Repository<UsertoDepartmentModel>, IUsertoDepartmentRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<UsertoDepartmentModel> _dbSet;
        public UsertoDepartmentRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<UsertoDepartmentModel>();
        }

        public List<string> GetDepartmentsbyUser(string UserID)
        {
            return _dbSet.Where(u => u.UserID == UserID).Select(u => u.DepartmanID).ToList();
        }

        public void RemoveDepartmentfromUser(string UserID)
        {
            string query = $"DELETE FROM `UsertoDepartment` WHERE `UserID` = '{UserID}'";
            var result = dbcontext.Database.ExecuteSqlRaw(query);
        }

        public void AddDepartmenttoUser(UsertoDepartmentModel model)
        {
            string query = $"INSERT INTO `UsertoDepartment` (`UserID`, `DepartmanID`) VALUES ('{model.UserID}','{model.DepartmanID}')";
            var result = dbcontext.Database.ExecuteSqlRaw(query);
        }
    }
}
