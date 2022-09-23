using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;
using System.Collections.Generic;
using System.Linq;

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

        public List<PatientdisabilitypermitformModel> GetDataByGuid(string Guid)
        {
            return _dbSet.Where(u => u.Activepatientid == Guid).ToList();
        }
    }
}