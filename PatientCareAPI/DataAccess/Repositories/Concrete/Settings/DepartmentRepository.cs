using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Settings;
using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Settings
{
    public class DepartmentRepository : Repository<DepartmentModel>, IDepartmentRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<DepartmentModel> _dbSet;
        public DepartmentRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<DepartmentModel>();
        }

        public DepartmentModel GetDepartmentByGuid(string guid)
        {
            return _dbSet.FirstOrDefault(u => u.ConcurrencyStamp == guid);
        }
    }
}
