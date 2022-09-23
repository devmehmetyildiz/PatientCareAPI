using PatientCareAPI.Models.Application;
using System.Collections.Generic;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Application
{
    public interface IPatientbodycontrolformRepository : IRepository<PatientbodycontrolformModel>
    {
        List<PatientbodycontrolformModel> GetDataByGuid(string Guid);
    }
}
