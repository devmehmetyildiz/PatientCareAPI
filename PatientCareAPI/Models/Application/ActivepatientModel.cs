using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Application
{
    public class ActivepatientModel : BaseModel
    {
        [StringLength(85)]
        public string PatientID { get; set; }
        [NotMapped]
        public PatientModel Patient { get; set; }
        public DateTime? Registerdate { get; set; }
        public DateTime? Releasedate { get; set; }
        public string Processid { get; set; }
        public string Floornumber { get; set; }
        public string Roomnumber { get; set; }
        [NotMapped]
        public ProcessModel Process { get; set; }
    }
}
