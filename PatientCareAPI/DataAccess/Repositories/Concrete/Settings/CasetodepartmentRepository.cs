using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Settings;
using PatientCareAPI.Models.Settings;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Settings
{
    public class CasetodepartmentRepository : Repository<CasetoDepartmentModel>, ICasetodepartmentRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<CasetoDepartmentModel> _dbSet;
        public CasetodepartmentRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<CasetoDepartmentModel>();
        }

        public void AddDepartments(List<DepartmentModel> List, string CaseGuid)
        {
            List<CasetoDepartmentModel> Data = new List<CasetoDepartmentModel>();
            foreach (var item in List)
            {
                Data.Add(new CasetoDepartmentModel { Id = 0, CaseID = CaseGuid, DepartmentID = item.ConcurrencyStamp });
            }
            _dbSet.AddRange(Data);
        }

        public void DeleteDepartmentsByCase(string CaseGuid)
        {
            _dbSet.RemoveRange(_dbSet.Where(u => u.CaseID == CaseGuid));
        }

        public void RemoveCasesfromDepartment(string CaseId)
        {
            string query = $"DELETE FROM `casetodepartments` WHERE `CaseID` = '{CaseId}'";
            var result = dbcontext.Database.ExecuteSqlRaw(query);
        }
    }
}
