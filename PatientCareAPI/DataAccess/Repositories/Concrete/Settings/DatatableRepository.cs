using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Settings;
using PatientCareAPI.Models.Settings;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Settings
{
    public class DatatableRepository : Repository<DatatableModel>, IDatatableRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<DatatableModel> _dbSet;
        public DatatableRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<DatatableModel>();
        }

        public DatatableModel GetDatatableByName(string name)
        {
            return dbcontext.Datatables.FirstOrDefault(u => u.Tablename == name);
        }
    }
}
