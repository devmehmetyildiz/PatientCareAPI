using PatientCareAPI.Models.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Application
{
    public class PatientdisabledhealthboardreportModel : PatientformbaseModel
    {
        public string Activestockid { get; set; }
        public string Reportno { get; set; }
        public string Reportname { get; set; }
        public string Sendinginstitution { get; set; }
        public DateTime? Appealdate { get; set; }
        public string Disabilityname { get; set; }
        public string Disabilityinfo { get; set; }
        public string Disabilityrate { get; set; }
        public string Disabilitystatus { get; set; }
        public string Wontworkjobs { get; set; }
        public string Ispermanent { get; set; }
        [NotMapped]
        public List<PatientdiagnosisModel> Diagnosies { get; set; }
    }
}
