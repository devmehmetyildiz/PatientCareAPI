using PatientCareAPI.Models.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Application
{
    public interface IStockmovementRepository : IRepository<StockmovementModel>
    {
        List<StockmovementModel> FindByStockGuid(string guid);
        List<StockmovementModel> FindByActivestockGuid(string guid);
    }
}
