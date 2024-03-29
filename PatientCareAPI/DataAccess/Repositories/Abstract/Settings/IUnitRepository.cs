﻿using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Settings
{
    public interface IUnitRepository : IRepository<UnitModel>
    {
        List<UnitModel> GetByUserDepartment(string username);
        UnitModel GetUnitByGuid(string guid);
        List<UnitModel> GetUnitsbyGuids(List<string> units);
    }
}
