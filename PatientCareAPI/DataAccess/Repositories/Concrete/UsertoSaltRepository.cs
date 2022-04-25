using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace PatientCareAPI.DataAccess.Repositories.Concrete
{
    public class UsertoSaltRepository : Repository<UsertoSaltModel>, IUsertoSaltRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<UsertoSaltModel> _dbSet;
        public UsertoSaltRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<UsertoSaltModel>();
        }

        public string GetSaltByGuid(string UserGuid)
        {
            return _dbSet.FirstOrDefault(u => u.UserID == UserGuid).Salt;
        }
    }
}