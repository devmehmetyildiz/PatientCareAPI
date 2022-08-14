using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;
using System.Linq;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Application
{
    public class ProcesstoFilesRepository : Repository<ProcesstoFilesModel>, IProcesstoFilesRepostiyory
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<ProcesstoFilesModel> _dbSet;
        public ProcesstoFilesRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<ProcesstoFilesModel>();
        }

        public void DeleteFilesByProcess(string ProcessGuid)
        {
            _dbSet.RemoveRange(_dbSet.Where(u => u.ProcessID == ProcessGuid));
        }
    }
}
