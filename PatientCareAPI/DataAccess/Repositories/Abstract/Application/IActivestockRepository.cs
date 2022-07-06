﻿using PatientCareAPI.Models.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Application
{
    public interface IActivestockRepository : IRepository<ActivestockModel>
    {
        ActivestockModel GetStockByGuid(string guid);

    }
}
