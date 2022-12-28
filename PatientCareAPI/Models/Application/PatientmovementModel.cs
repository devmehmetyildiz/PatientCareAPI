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
        public string Movementid { get; set; }
        [NotMapped]
        public PatientModel Patient { get; set; }
        public string Activepatientid { get; set; }
        [NotMapped]
        public string Movementtypename { get; set; }
        public int Movementtype { get; set; }
        public bool Iswaitingactivation { get; set; }
        public bool IsDeactive { get; set; }
        public string Oldstatus { get; set; }
        public string NewStatus { get; set; }
        public bool IsTodoneeded { get; set; }
        public string IsComplated { get; set; }
        public DateTime? Movementdate { get; set; }
        public string UserID { get; set; }
    }
}
