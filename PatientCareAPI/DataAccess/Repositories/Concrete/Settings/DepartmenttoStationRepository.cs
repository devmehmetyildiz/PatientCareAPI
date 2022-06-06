using Microsoft.EntityFrameworkCore;
using PatientCareAPI.DataAccess.Repositories.Abstract.Settings;
using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Concrete.Settings
{
    public class DepartmenttoStationRepository : Repository<DepartmenttoStationModel>, IDepartmenttoStationRepository
    {
        public ApplicationDBContext dbcontext { get { return _context as ApplicationDBContext; } }
        private DbSet<DepartmenttoStationModel> _dbSet;
        public DepartmenttoStationRepository(ApplicationDBContext context) : base(context)
        {
            _dbSet = dbcontext.Set<DepartmenttoStationModel>();
        }

        public List<string> GetStationsbyDepartment(string DepartmentID)
        {
            return _dbSet.Where(u => u.DepartmentID == DepartmentID).Select(u => u.StationID).ToList();
        }

        public void RemoveStationsfromDepartment(string DepartmentID)
        {
            string query = $"DELETE FROM `DepartmenttoStation` WHERE `DepartmentID` = '{DepartmentID}'";
            var result = dbcontext.Database.ExecuteSqlRaw(query);
        }

        public void AddStationstoDepartment(DepartmenttoStationModel model)
        {
            string query = $"INSERT INTO `DepartmenttoStation` (`DepartmentID`, `StationID`) VALUES ('{model.DepartmentID}','{model.StationID}')";
            var result = dbcontext.Database.ExecuteSqlRaw(query);
        }

    }
}