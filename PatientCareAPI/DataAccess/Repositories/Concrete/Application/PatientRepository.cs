using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Application
{
    public class PatientRepository : Repository<PatientModel>, IPatientRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<PatientModel> _dbSet;
        public PatientRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<PatientModel>();
        }

        public PatientModel GetPatientByGuid(string guid)
        {
            return _dbSet.FirstOrDefault(u => u.ConcurrencyStamp == guid);
        }
    }
}
