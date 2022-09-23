using PatientCareAPI.Models.Application;
using System.Collections.Generic;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Application
{
    public interface IPatientfirstapproachreportRepository : IRepository<PatientfirstapproachreportModel>
    {
        List<PatientfirstapproachreportModel> GetDataByGuid(string Guid);
    }
}
