using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Application
{
    public class PatientrecieveformRepository : Repository<PatientrecieveformModel>, IPatientrecieveformRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<PatientrecieveformModel> _dbSet;
        public PatientrecieveformRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<PatientrecieveformModel>();
        }

    }
}