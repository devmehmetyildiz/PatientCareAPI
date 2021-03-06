using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Settings
{
    public interface IStockRepository : IRepository<StockModel>
    {
        List<StockModel> GetByUserDepartment(string username);

        StockModel GetStockByGuid(string guid);
    }
}
