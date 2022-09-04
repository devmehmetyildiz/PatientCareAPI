using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;
using System.Linq;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Application
{
    public class ActivepatientRepository : Repository<ActivepatientModel>, IActivepatientRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<ActivepatientModel> _dbSet;
        public ActivepatientRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<ActivepatientModel>();
        }

        public ActivepatientModel FindByGuid(string guid)
        {
            return _dbSet.FirstOrDefault(u => u.ConcurrencyStamp == guid);
        }
    }
}
