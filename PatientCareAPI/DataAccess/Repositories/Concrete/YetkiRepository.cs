using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace PatientCareAPI.DataAccess.Repositories.Concrete
{
    public class YetkiRepository : Repository<YetkiModel>, IYetkiRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<YetkiModel> _dbSet;
        public YetkiRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<YetkiModel>();
        }

        public YetkiModel FindyetkiByName(string yetkiName)
        {
            return _dbSet.FirstOrDefault(u => u.NormalizedName == yetkiName.ToUpper());
        }

        public YetkiModel FindyetkiBuGuid(string Guid)
        {
            return _dbSet.FirstOrDefault(u => u.ConcurrencyStamp == Guid);
        }
    }
}
