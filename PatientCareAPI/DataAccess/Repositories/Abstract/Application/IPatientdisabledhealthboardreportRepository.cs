using PatientCareAPI.Models.Application;
using System.Collections.Generic;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Application
{
    public interface IPatientdisabledhealthboardreportRepository : IRepository<PatientdisabledhealthboardreportModel>
    {
        List<PatientdisabledhealthboardreportModel> GetDataByGuid(string Guid);
    }
}
