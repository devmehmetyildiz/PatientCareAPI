using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;
using System.Linq;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Application
{
    public class ProcesstoUsersRepository : Repository<ProcesstoUsersModel>, IProcesstoUsersRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<ProcesstoUsersModel> _dbSet;
        public ProcesstoUsersRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<ProcesstoUsersModel>();
        }

        public void DeleteUserByProcess(string ProcessGuid)
        {
            _dbSet.RemoveRange(_dbSet.Where(u => u.ProcessID == ProcessGuid));
        }
    }
}
