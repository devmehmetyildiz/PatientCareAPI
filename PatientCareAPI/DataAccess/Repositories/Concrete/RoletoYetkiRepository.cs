using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace PatientCareAPI.DataAccess.Repositories.Concrete
{
    public class RoletoYetkiRepository : Repository<RoletoYetki>, IRoletoYetkiRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<RoletoYetki> _dbSet;
        public RoletoYetkiRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<RoletoYetki>();
        }

        public void AddYetkitoRole(RoletoYetki model)
        {
            string query = $"INSERT INTO authory_to_roles (`AuthoryID`, `RoleID`) VALUES ('{model.RoleID}','{model.yetkiID}')";
            var result = dbcontext.Database.ExecuteSqlRaw(query);
        }

        

        public List<string> GetYetkisByRole(string RoleId)
        {
            return _dbSet.Where(u => u.RoleID == RoleId).Select(u => u.yetkiID).ToList();
        }

        public void DeleteYetkisbyRole(string RoleGuid)
        {
            string query = $"DELETE FROM authory_to_roles WHERE `AuthoryID` = '{RoleGuid}'";
            var result = dbcontext.Database.ExecuteSqlRaw(query);
        }
    }
}