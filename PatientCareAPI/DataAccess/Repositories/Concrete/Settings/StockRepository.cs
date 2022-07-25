using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Settings;
using PatientCareAPI.Models.Settings;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Settings
{
    public class StockRepository : Repository<StockModel>, IStockRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<StockModel> _dbSet;
        public StockRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<StockModel>();
        }

        public StockModel GetStockByGuid(string guid)
        {
            return _dbSet.FirstOrDefault(u => u.ConcurrencyStamp == guid);
        }
        public List<StockModel> GetByUserDepartment(string username)
        {
            string query = "";
            query+="select s.* from stocks s ";
            query +="left join departments d on s.Departmentid = d.ConcurrencyStamp ";
            query +="left join usertodepartment u on d.ConcurrencyStamp =u.DepartmanID ";
            query +="left join users u2 on u.UserID  = u2.ConcurrencyStamp ";
            query +=$"where u2.Username ='{username}' ";
            return _dbSet.FromSqlRaw(query).ToList(); ;
        }
    }
}
