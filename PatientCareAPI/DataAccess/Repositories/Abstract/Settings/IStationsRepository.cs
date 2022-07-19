using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Settings
{
    public interface IStationsRepository : IRepository<StationsModel>
    {
        List<StationsModel> GetStationsbyGuids(List<string> stations);
        StationsModel GetStationbyGuid(string guid);
    }
}
