using PatientCareAPI.Models.Application;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Application
{
    public interface IActivepatientRepository : IRepository<ActivepatientModel>
    {
        ActivepatientModel FindByGuid(string guid);
    }
}
