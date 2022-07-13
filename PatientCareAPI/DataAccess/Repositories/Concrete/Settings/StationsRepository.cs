using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Settings;
using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Settings
{
    public class StationsRepository : Repository<StationsModel>, IStationsRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<StationsModel> _dbSet;
        public StationsRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<StationsModel>();
        }

        public List<StationsModel> GetStationsbyGuids(List<string> stations)
        {
            if (stations.Count == 0)
            {
                return new List<StationsModel>();
            }
            string query = "";
            query+= "select * from stations  where ConcurrencyStamp IN (";
            for (int i = 0; i < stations.Count; i++)
            {
                query += $"'{stations[i]}'";
                if(i!=stations.Count-1)
                query += ",";
            }
            query += ")";
            return _dbSet.FromSqlRaw(query).ToList(); ;
        }
    }
}
