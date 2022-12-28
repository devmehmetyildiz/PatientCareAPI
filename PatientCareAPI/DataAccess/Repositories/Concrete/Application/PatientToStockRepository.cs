using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;
using System.Linq;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Application
{
    public class PatientToStockRepository : Repository<PatientToStockModel>, IPatientToStockRepostiyory
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<PatientToStockModel> _dbSet;
        public PatientToStockRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<PatientToStockModel>();
        }

        public void Deletestockbypatient(string Patientguid)
        {
            _dbSet.RemoveRange(_dbSet.Where(u => u.PatientID == Patientguid));
        }
    }
}
