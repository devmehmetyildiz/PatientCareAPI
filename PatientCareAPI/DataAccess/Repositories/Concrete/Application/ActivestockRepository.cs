using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;
using System.Linq;

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

        public ActivestockModel GetStockByGuid(string guid)
        {
            return _dbSet.FirstOrDefault(u => u.ConcurrencyStamp == guid);
        }
    }
}
