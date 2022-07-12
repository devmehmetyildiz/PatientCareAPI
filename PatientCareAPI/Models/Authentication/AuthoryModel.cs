using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Authentication
{
    public class AuthoryModel
    {
        [Key]      
        public int Id { get; set; }
        [StringLength(85)]
        public string Name { get; set; }
        [StringLength(85)]
        public string Group { get; set; }
        [StringLength(85)]
        public string ConcurrencyStamp { get; set; }
        [NotMapped]
        public bool isAdded { get; set; }
    }
}
