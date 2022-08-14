using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using PatientCareAPI.Models.Global;

namespace PatientCareAPI.Models.Application
{
    public class PatientfirstadmissionformModel : PatientformbaseModel
    {
        public string ActivepatientID { get; set; }
        public string Patienttype { get; set; }
        public string Locationknowledge { get; set; }
        public bool Ishaveitem { get; set; }
        public string Itemstxt { get; set; }
        [NotMapped]
        public List<string> Items { get; set; }
        public string Reportstatus { get; set; }
        public DateTime? Reportvaliddate { get; set; }
        public string Reportdegree { get; set; }
        public DateTime? Bodycontroldate { get; set; }
        public string Disableorientation { get; set; }
        public string Controllername { get; set; }
        public string Managername { get; set; }
    }
}
