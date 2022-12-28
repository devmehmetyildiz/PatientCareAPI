using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;
using System.Linq;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Application
{
    public class PatientdefineRepository : Repository<PatientdefineModel>, IPatientdefineRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<PatientdefineModel> _dbSet;
        public PatientdefineRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<PatientdefineModel>();
        }

        public PatientdefineModel GetPatientByGuid(string guid)
        {
            return _dbSet.FirstOrDefault(u => u.ConcurrencyStamp == guid);
        }
    }
}
