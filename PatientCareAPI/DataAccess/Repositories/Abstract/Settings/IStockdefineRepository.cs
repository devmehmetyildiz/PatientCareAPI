using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Settings
{
    public interface IStockdefineRepository : IRepository<StockdefineModel>
    {
        List<StockdefineModel> GetByUserDepartment(string username);

        StockdefineModel GetStockByGuid(string guid);
    }
}
