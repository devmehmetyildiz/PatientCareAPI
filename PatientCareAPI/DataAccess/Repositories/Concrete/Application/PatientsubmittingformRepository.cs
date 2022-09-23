using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;
using System.Collections.Generic;
using System.Linq;

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

        public List<PatientsubmittingformModel> GetDataByGuid(string Guid)
        {
            return _dbSet.Where(u => u.Activepatientid == Guid).ToList();
        }
    }
}