using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Authentication;

namespace PatientCareAPI.DataAccess.Repositories.Abstract
{
    public interface IYetkiRepository : IRepository<YetkiModel>
    {
        YetkiModel FindyetkiByName(string yetkiName);

        YetkiModel FindyetkiBuGuid(string Guid);
    }
}
