using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.DataAccess.Repositories.Abstract.Auth;
using Microsoft.EntityFrameworkCore;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Auth
{
    public class RoleRepository : Repository<RoleModel>, IRoleRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<RoleModel> _dbSet;
        public RoleRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<RoleModel>();
        }

        public RoleModel FindByName(string name)
        {
            return _dbSet.FirstOrDefault(x => x.Name == name);
        }

        public List<RoleModel> GetRolesbyGuids(List<string> roles)
        {
            if (roles.Count == 0)
            {
                return new List<RoleModel>();
            }
            string query = "";
            query += "select * from roles  where ConcurrencyStamp IN (";
            for (int i = 0; i < roles.Count; i++)
            {
                query += $"'{roles[i]}'";
                if (i != roles.Count - 1)
                    query += ",";
            }
            query += ")";
            return _dbSet.FromSqlRaw(query).ToList();
        }
    }
}