using PatientCareAPI.Models.Application;
using PatientCareAPI.Models.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Settings
{
    public class PatientModel : BaseModel
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string CountryID { get; set; }
        public string Patienttypeid { get; set; }
        [NotMapped]
        public string Patienttypetxt { get; set; }
        [NotMapped]
        public PatienttypeModel Patienttype { get; set; }
        public string Costumertypeid{ get; set; }
        [NotMapped]
        public string Costumertypetxt { get; set; }
        [NotMapped]
        public CostumertypeModel Costumertype { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Town { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime? Dateofbirth { get; set; }
        public DateTime? Dateofdeath{ get; set; }
    }
}
