using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Warehouse;
using PatientCareAPI.Models.Settings;
using PatientCareAPI.Models.Warehouse;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Warehouse
{
    public class StockdefineRepository : Repository<StockdefineModel>, IStockdefineRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<StockdefineModel> _dbSet;
        public StockdefineRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<StockdefineModel>();
        }

        public StockdefineModel GetStockByGuid(string guid)
        {
            return _dbSet.FirstOrDefault(u => u.ConcurrencyStamp == guid);
        }
        public List<StockdefineModel> GetByUserDepartment(string username)
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
