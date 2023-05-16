using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Application
{
    public class TodoModel: BaseModel
    {
        public string MovementID { get; set; }
        public string TododefineID { get; set; }
        [NotMapped]
        public PatientmovementModel Movement { get; set; }
        [NotMapped]
        public PatientModel Patient { get; set; }
        [NotMapped]
        public TododefineModel Tododefine { get; set; }
        public int Order { get; set; }
        public bool IsCompleted { get; set; }
    }
}
