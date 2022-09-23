using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;
using System.Linq;

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

        public PatientapplicantModel GetDatabyGuid(string guid)
        {
            return _dbSet.FirstOrDefault(u => u.Activepatientid == guid);
        }
    }
}
