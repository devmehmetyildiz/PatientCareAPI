using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Auth;
using PatientCareAPI.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Auth
{
    public class TablemetaconfigRepository : Repository<TablemetaconfigModel>, ITablemetaconfigRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<TablemetaconfigModel> _dbSet;
        public TablemetaconfigRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<TablemetaconfigModel>();
        }
    }
}
