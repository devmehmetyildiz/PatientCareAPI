using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Application
{
    public class PatientbodycontrolformRepository : Repository<PatientbodycontrolformModel>, IPatientbodycontrolformRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<PatientbodycontrolformModel> _dbSet;
        public PatientbodycontrolformRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<PatientbodycontrolformModel>();
        }

    }
}
