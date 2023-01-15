using PatientCareAPI.Models.Application;
using PatientCareAPI.Models.Warehouse;
using System.Collections.Generic;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Warehouse
{
    public interface IStockmovementRepository : IRepository<StockmovementModel>
    {
        List<StockmovementModel> FindByStockGuid(string guid);
        List<StockmovementModel> FindByActivestockGuid(string guid);
    }
}
