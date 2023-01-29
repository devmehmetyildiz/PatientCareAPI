using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Settings
{
    public class CheckperiodtoPeriodModel
    {
        [Key]
        public int Id { get; set; }
        public string CheckperiodID { get; set; }
        public string PeriodID { get; set; }
    }
}
