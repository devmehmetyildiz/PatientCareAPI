using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Settings;
using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Settings
{
    public class UnitRepository : Repository<UnitModel>, IUnitRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<UnitModel> _dbSet;
        public UnitRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<UnitModel>();
        }

        public UnitModel GetUnitByGuid(string guid)
        {
            return _dbSet.FirstOrDefault(u => u.ConcurrencyStamp == guid);
        }

        public List<UnitModel> GetByUserDepartment(string username)
        {
            string query = "";
            query += "select u.* from units u ";
            query += "left join unittodepartments u2 on u.ConcurrencyStamp = u2.UnitId ";
            query +="left join departments d on d.ConcurrencyStamp = u2.DepartmentId  ";
            query +="left join usertodepartment u3 on d.ConcurrencyStamp =u3.DepartmanID ";
            query +="left join users u4 on u3.UserID  = u4.ConcurrencyStamp ";
            query += $"where u4.Username ='{username}'";
            return _dbSet.FromSqlRaw(query).ToList(); ;
        }
    }
}
