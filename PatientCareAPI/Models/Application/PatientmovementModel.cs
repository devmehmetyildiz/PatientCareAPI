using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Application
{
    public class PatientmovementModel : BaseModel
    {
        public string PatientID { get; set; }
        [NotMapped]
        public PatientModel Patient { get; set; }
        public int Patientmovementtype { get; set; }
        public bool IsDeactive { get; set; }
        public int OldPatientmovementtype { get; set; }
        public int NewPatientmovementtype { get; set; }
        public bool IsTodoneed { get; set; }
        public bool IsTodocompleted { get; set; }
        public bool IsComplated { get; set; }
        public bool Iswaitingactivation { get; set; }
        public DateTime? Movementdate { get; set; }
    }
}
