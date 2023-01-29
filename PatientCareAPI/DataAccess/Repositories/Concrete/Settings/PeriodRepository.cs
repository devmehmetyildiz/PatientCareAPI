using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Settings;
using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Settings
{
    public class PeriodRepository : Repository<PeriodModel> , IPeriodRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<PeriodModel> _dbSet;
        public PeriodRepository(ApplicationDBContext context): base(context)
        {
            _dbSet = dbcontext.Set<PeriodModel>();
        }

        public List<PeriodModel> GetPeriodsbyGuids(List<string> periods)
        {
            if (periods.Count == 0)
            {
                return new List<PeriodModel>();
            }
            string query = "";
            query += "select * from Periods  where ConcurrencyStamp IN (";
            for (int i = 0; i < periods.Count; i++)
            {
                query += $"'{periods[i]}'";
                if (i != periods.Count - 1)
                    query += ",";
            }
            query += ")";
            return _dbSet.FromSqlRaw(query).ToList();
        }
    }
}
