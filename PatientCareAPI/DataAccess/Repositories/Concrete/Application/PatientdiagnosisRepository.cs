using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;
using System.Collections.Generic;
using System.Linq;

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

        public List<PatientdiagnosisModel> GetDataByGuid(string Guid)
        {
            return _dbSet.Where(u => u.Reportid == Guid).ToList();
        }
    }
}