using PatientCareAPI.Models.Application;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Application
{
    public interface IActivestockRepository : IRepository<ActivestockModel>
    {
        ActivestockModel GetStockByGuid(string guid);

    }
}
