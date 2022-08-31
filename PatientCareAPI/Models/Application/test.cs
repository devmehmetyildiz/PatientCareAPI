using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Application
{
    public class test
    {
        public string PatientID { get; set; }
        public PatientModel Patient { get; set; }
        public DateTime? Approvaldate { get; set; }
        public DateTime? Registerdate { get; set; }
        public string Patientdiagnosis { get; set; }
        public DateTime? Releasedate { get; set; }
        public int Roomnumber { get; set; }
        public int Floornumber { get; set; }
        public int Bednumber { get; set; }
        public bool Iswaitingactivation { get; set; }
    }
}
