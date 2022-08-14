using PatientCareAPI.Models.Application;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Application
{
    public interface IProcesstoActivestocksRepostiyory : IRepository<ProcesstoActiveStocksModel>
    {
        void DeleteActiveStocksByProcess(string ProcessGuid);
    }
}
