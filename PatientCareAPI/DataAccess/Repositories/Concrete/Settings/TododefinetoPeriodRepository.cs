using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Settings;
using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Settings
{
    public class TododefinetoPeriodRepository : Repository<TododefinetoPeriodModel> , ITododefinetoPeriodRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<TododefinetoPeriodModel> _dbSet;
        public TododefinetoPeriodRepository(ApplicationDBContext context) :base(context)
        {
            _dbSet = dbcontext.Set<TododefinetoPeriodModel>();
        }

        public void RemovePeriodsfromTododefines(string TododefineID)
        {
            string query = $"DELETE FROM `TododefinetoPeriods` WHERE `TododefineID` = '{TododefineID}'";
            var result = dbcontext.Database.ExecuteSqlRaw(query);
        }

      
    }
}
