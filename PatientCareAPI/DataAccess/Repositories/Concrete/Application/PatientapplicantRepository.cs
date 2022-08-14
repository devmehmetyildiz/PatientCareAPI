using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Application
{
    public class PatientapplicantRepository : Repository<PatientapplicantModel>, IPatientapplicantRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<PatientapplicantModel> _dbSet;
        public PatientapplicantRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<PatientapplicantModel>();
        }

    }
}
