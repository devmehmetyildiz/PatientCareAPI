using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;
using System.Collections.Generic;
using System.Linq;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Application
{
    public class PatientdisabledhealthboardreportRepository : Repository<PatientdisabledhealthboardreportModel>, IPatientdisabledhealthboardreportRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<PatientdisabledhealthboardreportModel> _dbSet;
        public PatientdisabledhealthboardreportRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<PatientdisabledhealthboardreportModel>();
        }

        public List<PatientdisabledhealthboardreportModel> GetDataByGuid(string Guid)
        {
            return _dbSet.Where(u => u.Activepatientid == Guid).ToList();
        }
    }
}