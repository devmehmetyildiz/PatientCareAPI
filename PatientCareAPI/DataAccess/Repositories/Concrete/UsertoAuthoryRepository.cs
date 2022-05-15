using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace PatientCareAPI.DataAccess.Repositories.Concrete
{
    public class UsertoAuthoryRepository : Repository<UsertoAuthoryModel>, IUsertoAuthoryRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<UsertoAuthoryModel> _dbSet;
        public UsertoAuthoryRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<UsertoAuthoryModel>();
        }

        public List<string> GetAuthsbyUser(string UserID)
        {
            return _dbSet.Where(u => u.UserID == UserID).Select(u => u.AuthoryID).ToList();
        }

        public void AddAuthtoUser(UsertoAuthoryModel model)
        {
            string query = $"INSERT INTO user_to_authory (`UserID`, `AuthoryID`) VALUES ('{model.UserID}','{model.AuthoryID}')";
            var result = dbcontext.Database.ExecuteSqlRaw(query);
        }
    }
}