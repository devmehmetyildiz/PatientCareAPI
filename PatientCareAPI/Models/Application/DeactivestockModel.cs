using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Application
{
    public class DeactivestockModel
    {
        [Key]
        public int Id { get; set; }
        [NotMapped]
        public string Stockname { get; set; }
        [NotMapped]
        public string Departmentname { get; set; }
        [NotMapped]
        public ActivestockModel Activestock { get; set; }
        public string Activestockid { get; set; }
        public DateTime? Createtime { get; set; }
        public string Createduser { get; set; }
        public double Amount { get; set; }
    }
}
