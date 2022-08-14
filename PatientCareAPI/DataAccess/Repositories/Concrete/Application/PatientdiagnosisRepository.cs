using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Application
{
    public class PatientdiagnosisRepository : Repository<PatientdiagnosisModel>, IPatientdiagnosisRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<PatientdiagnosisModel> _dbSet;
        public PatientdiagnosisRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<PatientdiagnosisModel>();
        }

    }
}