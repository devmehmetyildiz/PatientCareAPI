using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Settings
{
    public class CheckperiodModel : BaseModel
    {
        public string Name { get; set; }
        public List<PeriodModel> Periods { get; set; }
        public int Periodtype { get; set; }
        public string Occureddays { get; set; }
    }
}
