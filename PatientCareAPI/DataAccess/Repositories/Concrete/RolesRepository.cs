using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace PatientCareAPI.DataAccess.Repositories.Concrete
{
    public class RolesRepository : Repository<RolesModel>, IRolesRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<RolesModel> _dbSet;
        public RolesRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<RolesModel>();
        }

        public RolesModel FindRoleByName(string RoleName)
        {
            return _dbSet.FirstOrDefault(u => u.NormalizedName == RoleName.ToUpper());
        }

        public RolesModel FindRoleBuGuid(string Guid)
        {
            return _dbSet.FirstOrDefault(u => u.ConcurrencyStamp == Guid);
        }
    }
}
