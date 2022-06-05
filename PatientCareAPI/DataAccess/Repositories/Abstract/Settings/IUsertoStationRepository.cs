using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Settings
{
    public interface IUsertoStationRepository : IRepository<UsertoStationsModel>
    {
        List<string> GetStationsbyUser(string UserID);
        void RemoveStationsfromUser(string UserID);
        void AddStationtoUser(UsertoStationsModel model);
       
    }
}
