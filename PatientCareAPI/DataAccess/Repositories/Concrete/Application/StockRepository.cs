using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;
using System.Collections.Generic;
using System.Linq;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Application
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

        public List<StockModel> GetStocksbyGuids(List<string> stocks)
        {
            if (stocks.Count == 0)
            {
                return new List<StockModel>();
            }
            string query = "";
            query += "select * from stocks  where ConcurrencyStamp IN (";
            for (int i = 0; i < stocks.Count; i++)
            {
                query += $"'{stocks[i]}'";
                if (i != stocks.Count - 1)
                    query += ",";
            }
            query += ")";
            return _dbSet.FromSqlRaw(query).ToList();
        }
    }
}
