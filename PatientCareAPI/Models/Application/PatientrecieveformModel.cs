using PatientCareAPI.Models.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Application
{
    public class PatientrecieveformModel : PatientformbaseModel
    {
        public DateTime? Reportdate { get; set; }
        public string Itemstxt { get; set; }
        [NotMapped]
        public List<string> Items { get; set; }
        public string Submittername { get; set; }
        public string Submittercountryid { get; set; }
    }
}
