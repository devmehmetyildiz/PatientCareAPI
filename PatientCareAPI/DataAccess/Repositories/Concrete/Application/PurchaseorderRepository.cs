using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.Models.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Application
{
    public class PurchaseorderRepository : Repository<PurchaseorderModel>, IPurchaseorderRepository
    {
        public PurchaseorderRepository(ApplicationDBContext context) : base(context)
        {
        }
    }
}