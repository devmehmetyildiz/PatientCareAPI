using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using PatientCareAPI.Models.Global;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Application
{
    public class PatientsubmittingformModel : PatientformbaseModel
    {
        public string Activepatientid { get; set; }
        [NotMapped]
        public List<ActivestockModel> Stocks { get; set; }
        [NotMapped]
        public List<string> Items { get; set; }
        public string Itemstxt { get; set; }
        public string Submitterpersonelname { get; set; }
        public string Recievername { get; set; }
        public string Recievercountryno { get; set; }
    }
}
