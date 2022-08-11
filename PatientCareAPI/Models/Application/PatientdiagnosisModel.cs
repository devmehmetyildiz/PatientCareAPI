using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Application
{
    public class PatientdiagnosisModel : BaseModel
    {
        public string Reportid { get; set; }
        public string Diagnosisname{ get; set; }
        public string Diagnosisstatus { get; set; }
        public string Info { get; set; }
    }
}
