using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Application;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using Microsoft.EntityFrameworkCore;

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
