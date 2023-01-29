using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Settings;
using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Settings
{
    public class CheckperiodRepository : Repository<CheckperiodModel> , ICheckperiodRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<CheckperiodModel> _dbSet;
        public CheckperiodRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<CheckperiodModel>();
        }

        public List<CheckperiodModel> GetCheckperiodsbyGuids(List<string> checkperiods)
        {
            if (checkperiods.Count == 0)
            {
                return new List<CheckperiodModel>();
            }
            string query = "";
            query += "select * from Checkperiods  where ConcurrencyStamp IN (";
            for (int i = 0; i < checkperiods.Count; i++)
            {
                query += $"'{checkperiods[i]}'";
                if (i != checkperiods.Count - 1)
                    query += ",";
            }
            query += ")";
            return _dbSet.FromSqlRaw(query).ToList();
        }
    }
}
