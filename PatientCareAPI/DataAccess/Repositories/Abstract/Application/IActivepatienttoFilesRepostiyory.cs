using PatientCareAPI.Models.Application;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Application
{
    public interface IActivepatienttoFilesRepostiyory : IRepository<ActivepatienttofilesModel>
    {
        void DeleteFilesByActivepatient(string Activepatientguid);
    }
}
