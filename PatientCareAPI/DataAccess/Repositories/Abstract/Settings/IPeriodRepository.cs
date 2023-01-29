using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.DataAccess.Repositories.Abstract.Settings
{
    public interface IPeriodRepository  : IRepository<PeriodModel>
    {
        List<PeriodModel> GetPeriodsbyGuids(List<string> periods);
    }
}
