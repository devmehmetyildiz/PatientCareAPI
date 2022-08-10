using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Application;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using Microsoft.EntityFrameworkCore;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Application
{
    public class StockmovementRepository : Repository<StockmovementModel>, IStockmovementRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<StockmovementModel> _dbSet;
        public StockmovementRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<StockmovementModel>();
        }

        public List<StockmovementModel> FindByStockGuid(string guid)
        {
            string query = "select s.* from stockmovements s ";
            query += "left join activestock a on s.Activestockid = a.ConcurrencyStamp ";
            query += "left join stocks s2 on a.Stockid = s2.ConcurrencyStamp ";
            query += $"where s2.ConcurrencyStamp  = '{guid}'";
            return _dbSet.FromSqlRaw(query).ToList();
        }

        public List<StockmovementModel> FindByActivestockGuid(string guid)
        {
            return _dbSet.Where(u => u.Activestockid == guid).ToList();
        }
    }
}