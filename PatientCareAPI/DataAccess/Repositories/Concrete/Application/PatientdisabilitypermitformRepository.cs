using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Application
{
    public class PatientdisabilitypermitformRepository : Repository<PatientdisabilitypermitformModel>, IPatientdisabilitypermitformRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<PatientdisabilitypermitformModel> _dbSet;
        public PatientdisabilitypermitformRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<PatientdisabilitypermitformModel>();
        }

    }
}