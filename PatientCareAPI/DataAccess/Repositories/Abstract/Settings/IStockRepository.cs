﻿using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Settings
{
    public interface IStockRepository : IRepository<StockModel>
    {
        StockModel GetStockByGuid(string guid);
    }
}
