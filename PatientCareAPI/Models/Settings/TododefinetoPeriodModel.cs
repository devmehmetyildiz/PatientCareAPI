using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Settings
{
    public class TododefinetoPeriodModel
    {
        [Key]
        public int Id { get; set; }
        public string TododefineID { get; set; }
        public string PeriodID { get; set; }
    }
}
