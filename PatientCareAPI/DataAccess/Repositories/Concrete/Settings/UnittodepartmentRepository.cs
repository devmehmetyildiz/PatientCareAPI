using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Settings;
using PatientCareAPI.Models.Settings;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Settings
{
    public class UnittodepartmentRepository : Repository<UnittoDepartmentModel>, IUnittodepartmentRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<UnittoDepartmentModel> _dbSet;
        public UnittodepartmentRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<UnittoDepartmentModel>();
        }
    }
}
