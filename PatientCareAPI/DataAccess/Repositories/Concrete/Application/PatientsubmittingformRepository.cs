using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Application
{
    public class PatientsubmittingformRepository : Repository<PatientsubmittingformModel>, IPatientsubmittingformRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<PatientsubmittingformModel> _dbSet;
        public PatientsubmittingformRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<PatientsubmittingformModel>();
        }

    }
}