using PatientCareAPI.Models.Application;
using System.Collections.Generic;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Application
{
    public interface IPatientrecieveformRepository : IRepository<PatientrecieveformModel>
    {
        List<PatientrecieveformModel> GetDataByGuid(string Guid);
    }
}
