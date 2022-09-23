using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;
using System.Collections.Generic;
using System.Linq;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Application
{
    public class PatientownershiprecieveRepository : Repository<PatientownershiprecieveModel>, IPatientownershiprecieveRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<PatientownershiprecieveModel> _dbSet;
        public PatientownershiprecieveRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<PatientownershiprecieveModel>();
        }
        public List<PatientownershiprecieveModel> GetDataByGuid(string Guid)
        {
            return _dbSet.Where(u => u.Activepatientid == Guid).ToList();
        }
    }
}