using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Settings;
using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Settings
{
    public class CaseRepository : Repository<CaseModel>, ICaseRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<CaseModel> _dbSet;
        public CaseRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<CaseModel>();
        }

        public List<CaseModel> GetByUserDepartment(string username)
        {
            string query = "";
            query += "select c.* from cases c ";
            query += "left join casetodepartments c2 on c.ConcurrencyStamp = c2.CaseID ";
            query += "left join departments d on d.ConcurrencyStamp = c2.DepartmentID ";
            query += "left join usertodepartment u on d.ConcurrencyStamp =u.DepartmanID ";
            query += "left join users u2 on u.UserID = u2.ConcurrencyStamp ";
            query +=  $"where u2.Username ='{username}' ";
            return _dbSet.FromSqlRaw(query).ToList(); ;
        }

        public List<CaseModel> GetCasesbyGuids(List<string> cases)
        {
            if (cases.Count == 0)
            {
                return new List<CaseModel>();
            }
            string query = "";
            query += "select * from cases  where ConcurrencyStamp IN (";
            for (int i = 0; i < cases.Count; i++)
            {
                query += $"'{cases[i]}'";
                if (i != cases.Count - 1)
                    query += ",";
            }
            query += ")";
            return _dbSet.FromSqlRaw(query).ToList();
        }
    }
}
