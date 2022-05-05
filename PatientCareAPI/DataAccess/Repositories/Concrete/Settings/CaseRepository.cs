using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Settings;
using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Settings
{
    public class CaseRepository : Repository<CaseModel>, ICaseRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<CaseModel> _dbSet;
        public CaseRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<CaseModel>();
        }
    }
}
