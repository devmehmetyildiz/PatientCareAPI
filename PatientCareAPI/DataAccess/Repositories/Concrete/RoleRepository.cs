using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace PatientCareAPI.DataAccess.Repositories.Concrete
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
    }
}