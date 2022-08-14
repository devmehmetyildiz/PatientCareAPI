using PatientCareAPI.Models.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Application
{
    public class PatientapplicantModel : BaseModel
    {
        public string Activepatientid { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Proximitystatus { get; set; }
        public string Countryid { get; set; }
        public string Fathername { get; set; }
        public string Mothername { get; set; }
        public DateTime? Dateofbirth { get; set; }
        public string Placeofbirth { get; set; }
        public string Gender { get; set; }
        public string Marialstatus { get; set; }
        public string Jobstatus { get; set; }
        public string Educationstatus { get; set; }
        public string Montlyincome { get; set; }
        public string Town { get; set; }
        public string City { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Country { get; set; }
        public string Contactnumber1 { get; set; }
        public string Contactnumber2 { get; set; }
        public string Contactname1 { get; set; }
        public string Contactname2 { get; set; }
        public DateTime? Appialdate { get; set; }
        public string Appialreason { get; set; }
    }
}
