using PatientCareAPI.Models.Application;
using PatientCareAPI.Models.Warehouse;
using System.Collections.Generic;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Warehouse
{
    public interface IStockRepository : IRepository<StockModel>
    {
        StockModel GetStockByGuid(string guid);

        List<StockModel> GetStocksbyGuids(List<string> stocks);
        void RemoveStocksbyGuids(List<string> stocks);
    }
}
