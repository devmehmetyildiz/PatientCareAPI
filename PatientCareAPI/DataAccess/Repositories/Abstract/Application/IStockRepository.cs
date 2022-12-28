using PatientCareAPI.Models.Application;
using System.Collections.Generic;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Application
{
    public interface IStockRepository : IRepository<StockModel>
    {
        StockModel GetStockByGuid(string guid);

        List<StockModel> GetStocksbyGuids(List<string> stocks);

    }
}
