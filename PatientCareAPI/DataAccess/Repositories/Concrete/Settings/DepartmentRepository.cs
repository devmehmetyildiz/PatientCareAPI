using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Settings;
using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Settings
{
    public class DepartmentRepository : Repository<DepartmentModel>, IDepartmentRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<DepartmentModel> _dbSet;
        public DepartmentRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<DepartmentModel>();
        }

        public DepartmentModel GetDepartmentByGuid(string guid)
        {
            return _dbSet.FirstOrDefault(u => u.ConcurrencyStamp == guid);
        }

        public List<DepartmentModel> GetDepartmentsbyGuids(List<string> departments)
        {
            if (departments.Count == 0)
            {
                return new List<DepartmentModel>();
            }
            string query = "";
            query += "select * from departments  where ConcurrencyStamp IN (";
            for (int i = 0; i < departments.Count; i++)
            {
                query += $"'{departments[i]}'";
                if (i != departments.Count - 1)
                    query += ",";
            }
            query += ")";
            return _dbSet.FromSqlRaw(query).ToList();
        }

        public List<DepartmentModel> GetDepartmentByUserDepartment(string username)
        {
            string query = "";
            query+="select d.* from departments d ";
            query +="left join usertodepartment u on d.ConcurrencyStamp = u.DepartmanID ";
            query +="left join users u2 on u.UserID =u2.ConcurrencyStamp  ";
            query +=$"where u2.Username ='{username}' ";
            return _dbSet.FromSqlRaw(query).ToList(); 
        }
    }
}
