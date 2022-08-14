using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Application
{
    public class ProcessRepository : Repository<ProcessModel>, IProcessRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<ProcessModel> _dbSet;
        public ProcessRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<ProcessModel>();
        }
    }
}
