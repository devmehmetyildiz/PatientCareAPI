﻿using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Settings
{
    public interface IDepartmenttoStationRepository : IRepository<DepartmenttoStationModel>
    {
        List<string> GetStationsbyDepartment(string DepartmentID);
        void RemoveStationsfromDepartment(string DepartmentID);
        void AddStationstoDepartment(DepartmenttoStationModel model);
    }
}
