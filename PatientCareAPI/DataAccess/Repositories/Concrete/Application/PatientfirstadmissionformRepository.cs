using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Application
{
    public class PatientfirstadmissionformRepository : Repository<PatientfirstadmissionformModel>, IPatientfirstadmissionformRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<PatientfirstadmissionformModel> _dbSet;
        public PatientfirstadmissionformRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<PatientfirstadmissionformModel>();
        }

    }
}