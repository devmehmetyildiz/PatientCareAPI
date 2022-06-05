using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Settings;
using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Settings
{
    public class UsertoStationRepository : Repository<UsertoStationsModel>, IUsertoStationRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<UsertoStationsModel> _dbSet;
        public UsertoStationRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<UsertoStationsModel>();
        }

        public List<string> GetStationsbyUser(string UserID)
        {
            return _dbSet.Where(u => u.UserID == UserID).Select(u => u.StationID).ToList();
        }

        public void RemoveStationsfromUser(string UserID)
        {
            string query = $"DELETE FROM `UsertoStations` WHERE `UserID` = '{UserID}'";
            var result = dbcontext.Database.ExecuteSqlRaw(query);
        }

        public void AddStationtoUser(UsertoStationsModel model)
        {
            string query = $"INSERT INTO `UsertoStations` (`UserID`, `StationID`) VALUES ('{model.UserID}','{model.StationID}')";
            var result = dbcontext.Database.ExecuteSqlRaw(query);
        }
    }
}
