using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Settings
{
    public class PeriodModel : BaseModel
    {
        public string Name { get; set; }
        public string Occuredtime { get; set; }
        public string Checktime { get; set; }
    }
}
