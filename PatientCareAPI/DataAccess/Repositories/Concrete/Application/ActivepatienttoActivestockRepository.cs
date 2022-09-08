using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;
using System.Linq;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Application
{
    public class ActivepatienttoActivestockRepository : Repository<ActivepatienttoActiveStocksModel>, IActivepatienttoActivestocksRepostiyory
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<ActivepatienttoActiveStocksModel> _dbSet;
        public ActivepatienttoActivestockRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<ActivepatienttoActiveStocksModel>();
        }

        public void DeleteActiveStocksByActivepatient(string Activepatientguid)
        {
            _dbSet.RemoveRange(_dbSet.Where(u => u.ActivepatientID == Activepatientguid));
        }
    }
}
