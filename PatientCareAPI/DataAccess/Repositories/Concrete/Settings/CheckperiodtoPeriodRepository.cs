using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Settings;
using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Settings
{
    public class CheckperiodtoPeriodRepository : Repository<CheckperiodtoPeriodModel> , ICheckperiodtoPeriodRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<CheckperiodtoPeriodModel> _dbSet;
        public CheckperiodtoPeriodRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<CheckperiodtoPeriodModel>();
        }

        public void RemovePeriodsfromCheckperiod(string CheckperiodID)
        {
            string query = $"DELETE FROM `Checkperiodstoperiods` WHERE `CheckperiodID` = '{CheckperiodID}'";
            var result = dbcontext.Database.ExecuteSqlRaw(query);
        }
    }
}
