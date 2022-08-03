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

    }
}