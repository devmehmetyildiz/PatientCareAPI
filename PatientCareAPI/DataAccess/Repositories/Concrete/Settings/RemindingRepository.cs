using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Settings;
using PatientCareAPI.Models.Settings;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Settings
{
    public class RemindingRepository : Repository<RemindingModel>, IRemindingRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<RemindingModel> _dbSet;
        public RemindingRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<RemindingModel>();
        }
    }
}
