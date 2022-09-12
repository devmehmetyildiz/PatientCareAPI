using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Application
{
    public class ActivepatientmovementstotodosModel
    {
        [Key]
        public int Id { get; set; }
        public int Vieworder { get; set; }
        public string MovementID { get; set; }
        public string TodoID { get; set; }
        public string Iscomplated { get; set; }
        public string ComplatedUser { get; set; }
        public  DateTime? Complatetime { get; set; }
    }
}
