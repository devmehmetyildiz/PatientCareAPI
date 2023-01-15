using PatientCareAPI.DataAccess.Repositories.Abstract.Application;
using PatientCareAPI.DataAccess.Repositories.Abstract.Warehouse;
using PatientCareAPI.Models.Application;
using PatientCareAPI.Models.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Warehouse
{
    public class PurchaseorderRepository : Repository<PurchaseorderModel>, IPurchaseorderRepository
    {
        public PurchaseorderRepository(ApplicationDBContext context) : base(context)
        {
        }
    }
}