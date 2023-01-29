using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Settings;
using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Settings
{
    public class TodogroupdefineRepository : Repository<TodogroupdefineModel> , ITodogroupdefineRepository
    {

        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<TodogroupdefineModel> _dbSet;
        public TodogroupdefineRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<TodogroupdefineModel>();
        }

        public List<TodogroupdefineModel> GetGroupsbyGuids(List<string> groups)
        {
            if (groups.Count == 0)
            {
                return new List<TodogroupdefineModel>();
            }
            string query = "";
            query += "select * from Todogroupdefines  where ConcurrencyStamp IN (";
            for (int i = 0; i < groups.Count; i++)
            {
                query += $"'{groups[i]}'";
                if (i != groups.Count - 1)
                    query += ",";
            }
            query += ")";
            return _dbSet.FromSqlRaw(query).ToList();
        }

    }
}
