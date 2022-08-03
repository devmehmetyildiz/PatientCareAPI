using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Application
{
    public class PatientmovementModel
    {
        [Key]
        public int Id { get; set; }
        [NotMapped]
        public string Patientname { get; set; }
        public string Patientid { get; set; }
        [NotMapped]
        public string Movementtypename { get; set; }
        public int Movementtype { get; set; }
        public DateTime? Movementdate { get; set; }
    }
}
