using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.DataAccess.Repositories.Abstract;
using PatientCareAPI.Utils;
using Microsoft.EntityFrameworkCore;

namespace PatientCareAPI.DataAccess.Repositories.Concrete
{
    public class UsersRepository : Repository<UsersModel>, IUsersRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<UsersModel> _dbSet;
        public UsersRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<UsersModel>();
        }

        public UsersModel FindUserByName(string name)
        {
            return _dbSet.FirstOrDefault(u => u.NormalizedUsername == name.ToUpper());
        }

        //public UsersModel FindUserByPassword(string password)
        //{
        //   return _dbSet.FirstOrDefault(u=>u.)
        //}
    }
}
