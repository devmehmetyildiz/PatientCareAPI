using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Application
{
    public class PatientfirstapproachreportRepository : Repository<PatientfirstapproachreportModel>, IPatientfirstapproachreportRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<PatientfirstapproachreportModel> _dbSet;
        public PatientfirstapproachreportRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<PatientfirstapproachreportModel>();
        }

    }
}