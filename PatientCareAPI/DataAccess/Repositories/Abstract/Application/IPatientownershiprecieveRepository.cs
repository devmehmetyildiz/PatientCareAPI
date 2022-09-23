using PatientCareAPI.Models.Application;
using System.Collections.Generic;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Application
{
    public interface IPatientownershiprecieveRepository : IRepository<PatientownershiprecieveModel>
    {
        List<PatientownershiprecieveModel> GetDataByGuid(string Guid);
    }
}
