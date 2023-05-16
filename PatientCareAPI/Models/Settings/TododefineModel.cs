using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Settings
{
    public class TododefineModel : BaseModel
    {
        public TododefineModel()
        {
            Periods = new List<PeriodModel>();
        }
        public string Name { get; set; }
        public string Info { get; set; }
        public bool IsRequired { get; set; }
        public bool IsNeedactivation { get; set; }
        [NotMapped]
        public List<PeriodModel> Periods { get; set; }
    }
}
