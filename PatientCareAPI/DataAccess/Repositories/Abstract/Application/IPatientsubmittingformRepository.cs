using PatientCareAPI.Models.Application;
using System.Collections.Generic;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Application
{
    public interface IPatientsubmittingformRepository : IRepository<PatientsubmittingformModel>
    {
        List<PatientsubmittingformModel> GetDataByGuid(string Guid);
    }
}
