using PatientCareAPI.Models.Application;
using System.Collections.Generic;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Application
{
    public interface IStockmovementRepository : IRepository<StockmovementModel>
    {
        List<StockmovementModel> FindByStockGuid(string guid);
        List<StockmovementModel> FindByActivestockGuid(string guid);
    }
}
