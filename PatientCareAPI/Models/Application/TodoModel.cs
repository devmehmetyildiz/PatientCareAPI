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
        public string TodoID { get; set; }
        [NotMapped]
        public TododefineModel Todo { get; set; }
        public int Order { get; set; }
        public bool IsCompleted { get; set; }
    }
}
