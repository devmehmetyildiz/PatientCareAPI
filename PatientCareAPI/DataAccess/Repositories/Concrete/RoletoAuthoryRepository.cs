using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace PatientCareAPI.DataAccess.Repositories.Concrete
{
    public class RoletoAuthoryRepository : Repository<RoletoAuthory>, IRoletoAuthoryRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<RoletoAuthory> _dbSet;
        public RoletoAuthoryRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<RoletoAuthory>();
        }

        public void AddAuthorytoRole(RoletoAuthory model)
        {
            string query = $"INSERT INTO roletoauthories (`RoleID`, `AuthoryID`) VALUES ('{model.RoleID}','{model.RoleID}')";
            var result = dbcontext.Database.ExecuteSqlRaw(query);
        }

        

        public List<string> GetAuthoriesByRole(string RoleId)
        {
            return _dbSet.Where(u => u.RoleID == RoleId).Select(u => u.RoleID).ToList();
        }

        public void DeleteAuthoriesbyRole(string RoleGuid)
        {
            string query = $"DELETE FROM roletoauthories WHERE `RoleID` = '{RoleGuid}'";
            var result = dbcontext.Database.ExecuteSqlRaw(query);
        }
    }
}