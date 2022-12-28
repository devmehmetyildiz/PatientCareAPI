using PatientCareAPI.Models.Application;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Application
{
    public interface IPatientToStockRepostiyory : IRepository<PatientToStockModel>
    {
        void Deletestockbypatient(string Patientguid);
    }
}
