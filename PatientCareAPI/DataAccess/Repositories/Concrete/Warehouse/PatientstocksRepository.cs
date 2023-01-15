using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Warehouse;
using PatientCareAPI.Models.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Warehouse
{
    public class PatientstocksRepository : Repository<PatientstocksModel>, IPatientstocksRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<PatientstocksModel> _dbSet;
        public PatientstocksRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<PatientstocksModel>();
        }

    }
}