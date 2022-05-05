using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Settings
{
    public class UnitModel : BaseModel
    {
        public string UnitGroup { get; set; }

        public int UnitStatus { get; set; }
    }
}
