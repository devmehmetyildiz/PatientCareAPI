using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Settings
{
    public class CheckperiodModel : BaseModel
    {
        public CheckperiodModel()
        {
            Periods = new List<PeriodModel>();
        }
        public string Name { get; set; }
        [NotMapped]
        public List<PeriodModel> Periods { get; set; }
        public string Periodtype { get; set; }
        public string Occureddays { get; set; }
    }
}
