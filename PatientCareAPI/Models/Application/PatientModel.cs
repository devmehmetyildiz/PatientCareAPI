using PatientCareAPI.Models.Settings;
using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Models.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Application
{
    public class PatientModel : BaseModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Fathername { get; set; }
        public string Mothername { get; set; }
        public string Motherbiologicalaffinity { get; set; }
        public bool Ismotheralive { get; set; }
        public string Fatherbiologicalaffinity { get; set; }
        public bool Isfatheralive { get; set; }
        public string CountryID { get; set; }
        public DateTime? Dateofbirth { get; set; }
        public string Placeofbirth { get; set; }
        public DateTime? Dateofdeath { get; set; }
        public string Placeofdeath { get; set; }
        public string Deathinfo { get; set; }
        public string Gender { get; set; }
        public string Marialstatus { get; set; }
        public string Criminalrecord { get; set; }
        public int Childnumber { get; set; }
        public int Disabledchildnumber { get; set; }
        public string Siblingstatus { get; set; }
        public string Sgkstatus { get; set; }
        public string Budgetstatus { get; set; }
        public string Town { get; set; }
        public string City { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Country { get; set; }
        public string Contactnumber1 { get; set; }
        public string Contactnumber2 { get; set; }
        public string Contactname1 { get; set; }
        public string Contactname2 { get; set; }
        public string Costumertypeid{ get; set; }
        public string Patienttypeid { get; set; }
        [NotMapped]
        public CostumertypeModel Costumertype { get; set; }
        [NotMapped]
        public PatienttypeModel Patienttype { get; set; }
        [NotMapped]
        public string Patienttypetxt { get; set; }
        [NotMapped]
        public string Costumertypetxt { get; set; }
    }
}
