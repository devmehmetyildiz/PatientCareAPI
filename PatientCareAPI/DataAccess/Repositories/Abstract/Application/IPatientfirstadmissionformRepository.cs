using PatientCareAPI.Models.Application;
using System.Collections.Generic;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Application
{
    public interface IPatientfirstadmissionformRepository : IRepository<PatientfirstadmissionformModel>
    {
        List<PatientfirstadmissionformModel> GetDataByGuid(string Guid);
    }
}
