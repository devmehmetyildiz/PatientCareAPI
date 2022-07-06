using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Application;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using Microsoft.EntityFrameworkCore;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Application
{
    public class ProcesstoActivestockRepository : Repository<ProcesstoActiveStocksModel>, IProcesstoActivestocksRepostiyory
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<ProcesstoActiveStocksModel> _dbSet;
        public ProcesstoActivestockRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<ProcesstoActiveStocksModel>();
        }

        public void DeleteActiveStocksByProcess(string ProcessGuid)
        {
            _dbSet.RemoveRange(_dbSet.Where(u => u.ProcessID == ProcessGuid));
        }
    }
}
