using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.DataAccess.Repositories.Abstract.Warehouse;
using PatientCareAPI.Models.Application;
using PatientCareAPI.Models.Warehouse;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Warehouse
{
    public class DeactivestockRepository : Repository<DeactivestockModel>, IDeactivestockRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<DeactivestockModel> _dbSet;
        public DeactivestockRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<DeactivestockModel>();
        }

    }
}
