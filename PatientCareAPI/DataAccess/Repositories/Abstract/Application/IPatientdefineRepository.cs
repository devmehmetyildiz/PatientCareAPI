using PatientCareAPI.Models.Application;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Application
{
    public interface IPatientdefineRepository : IRepository<PatientdefineModel>
    {
        PatientdefineModel GetPatientByGuid(string guid);
    }
}
