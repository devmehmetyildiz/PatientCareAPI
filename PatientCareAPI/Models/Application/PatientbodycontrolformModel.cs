using PatientCareAPI.Models.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Application
{
    public class PatientbodycontrolformModel : PatientformbaseModel
    {
        public string Activepatientid { get; set; }
        public string Info { get; set; }
        public string Checkreason { get; set; }
        public string Controllername { get; set; }
        public string Cotrollername1 { get; set; }
        public string Controllername2 { get; set; }
    }
}
