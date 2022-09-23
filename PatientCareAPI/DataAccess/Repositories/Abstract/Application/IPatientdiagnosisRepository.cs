using PatientCareAPI.Models.Application;
using System.Collections.Generic;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Application
{
    public interface IPatientdiagnosisRepository : IRepository<PatientdiagnosisModel>
    {
        List<PatientdiagnosisModel> GetDataByGuid(string Guid);
    }
}
