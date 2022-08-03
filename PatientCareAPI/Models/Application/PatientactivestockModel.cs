using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Application
{
    public class PatientactivestockModel
    {
        [Key]
        public int Id { get; set; }
        public string Activestockid { get; set; }
        public DateTime? Createtime { get; set; }
        public string Createduser { get; set; }
        public string Amount { get; set; }
    }
}
