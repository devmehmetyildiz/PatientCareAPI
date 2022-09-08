using PatientCareAPI.Models.Application;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Application
{
    public interface IActivepatienttoActivestocksRepostiyory : IRepository<ActivepatienttoActiveStocksModel>
    {
        void DeleteActiveStocksByActivepatient(string Activepatientguid);
    }
}
