using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Application;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using Microsoft.EntityFrameworkCore;

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
    }
}
