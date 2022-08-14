using PatientCareAPI.Models.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Application
{
    public class PatientownershiprecieveModel : PatientformbaseModel
    {
        public string Activepatientid { get; set; }
        public string Itemstxt { get; set; }
        [NotMapped]
        public List<string> Items { get; set; }
        public string Recievername { get; set; }
        public string Recievercountryno { get; set; }
        public string Submittercountryno { get; set; }
        public string Submittername { get; set; }
        public string Witnessname { get; set; }
        public string Witnesscountryid { get; set; }
    }
}
