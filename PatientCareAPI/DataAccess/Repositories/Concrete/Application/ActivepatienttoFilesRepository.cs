using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;
using System.Linq;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Application
{
    public class ActivepatienttoFilesRepository : Repository<ActivepatienttofilesModel>, IActivepatienttoFilesRepostiyory
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<ActivepatienttofilesModel> _dbSet;
        public ActivepatienttoFilesRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<ActivepatienttofilesModel>();
        }

        public void DeleteFilesByActivepatient(string Activepatientguid)
        {
            _dbSet.RemoveRange(_dbSet.Where(u => u.ActivepatientID == Activepatientguid));
        }
    }
}
