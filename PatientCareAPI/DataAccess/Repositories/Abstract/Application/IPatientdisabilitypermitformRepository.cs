using PatientCareAPI.Models.Application;
using System.Collections.Generic;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Application
{
    public interface IPatientdisabilitypermitformRepository : IRepository<PatientdisabilitypermitformModel>
    {
        List<PatientdisabilitypermitformModel> GetDataByGuid(string Guid);
    }
}
