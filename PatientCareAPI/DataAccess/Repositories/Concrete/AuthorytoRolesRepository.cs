using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace PatientCareAPI.DataAccess.Repositories.Concrete
{
    public class AuthorytoRolesRepository : Repository<AuthorytoRoles>, IAuthorytoRolesRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<AuthorytoRoles> _dbSet;
        public AuthorytoRolesRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<AuthorytoRoles>();
        }

        public void AddRoletoAuth(AuthorytoRoles model)
        {
            string query = $"INSERT INTO authory_to_roles (`AuthoryID`, `RoleID`) VALUES ('{model.AuthoryID}','{model.RoleID}')";
            var result = dbcontext.Database.ExecuteSqlRaw(query);
        }

        public List<string> GetRolesByAuth(string AuthId)
        {
            return _dbSet.Where(u => u.AuthoryID == AuthId).Select(u => u.RoleID).ToList();
        }
    }
}