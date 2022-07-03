using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Application;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using Microsoft.EntityFrameworkCore;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Application
{
    public class ActivestockRepository : Repository<ActivestockModel>, IActivestockRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<ActivestockModel> _dbSet;
        public ActivestockRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<ActivestockModel>();
        }
    }
}
