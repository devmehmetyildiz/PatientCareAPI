using PatientCareAPI.Models.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Application
{
    public class PatientfirstapproachreportModel : PatientformbaseModel
    {
        public string ActivepatientID { get; set; }
        public DateTime? Acceptancedate { get; set; }
        public DateTime? Interviewdate { get; set; }
        public DateTime? Healthinitialassesmentdate { get; set; }
        public string Reasonforhealtcare { get; set; }
        public string Ratinginfo { get; set; }
        public string Knowledgesource { get; set; }
        public string Controllername { get; set; }
    }
}
