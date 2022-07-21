using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Settings;
using PatientCareAPI.Models.Settings;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Settings
{
    public class CostumertypetoDepartmentRepository : Repository<CostumertypetoDepartmentModel>, ICostumertypetoDepartmentRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<CostumertypetoDepartmentModel> _dbSet;
        public CostumertypetoDepartmentRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<CostumertypetoDepartmentModel>();
        }

        public void AddDepartments(List<DepartmentModel> List, string CostumertypeGuid)
        {
            List<CostumertypetoDepartmentModel> Data = new List<CostumertypetoDepartmentModel>();
            foreach (var item in List)
            {
                Data.Add(new CostumertypetoDepartmentModel { Id = 0, CostumertypeID = CostumertypeGuid, DepartmentID = item.ConcurrencyStamp });
            }
            _dbSet.AddRange(Data);
        }

        public void DeleteDepartmentsByCostumertype(string CostumertypeGuid)
        {
            _dbSet.RemoveRange(_dbSet.Where(u => u.CostumertypeID == CostumertypeGuid));
        }
    }
}