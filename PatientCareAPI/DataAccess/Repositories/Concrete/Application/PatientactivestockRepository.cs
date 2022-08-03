using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Application;
using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using Microsoft.EntityFrameworkCore;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Application
{
    public class PatientactivestockRepository : Repository<PatientactivestockModel>, IPatientactivestockRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<PatientactivestockModel> _dbSet;
        public PatientactivestockRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<PatientactivestockModel>();
        }

    }
}