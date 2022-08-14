using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Application
{
    public class PatientownershiprecieveRepository : Repository<PatientownershiprecieveModel>, IPatientownershiprecieveRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<PatientownershiprecieveModel> _dbSet;
        public PatientownershiprecieveRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<PatientownershiprecieveModel>();
        }

    }
}