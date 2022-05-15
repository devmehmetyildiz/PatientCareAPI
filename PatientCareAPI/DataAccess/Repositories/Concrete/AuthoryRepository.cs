using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace PatientCareAPI.DataAccess.Repositories.Concrete
{
    public class AuthoryRepository : Repository<AuthoryModel>, IAuthoryRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<AuthoryModel> _dbSet;
        public AuthoryRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<AuthoryModel>();
        }

        public AuthoryModel FindByName(string name)
        {
            return _dbSet.FirstOrDefault(x => x.Name == name);
        }
    }
}