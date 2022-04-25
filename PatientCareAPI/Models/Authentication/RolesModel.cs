using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Authentication
{
    public class RolesModel
    {
        [Key]      
        public int Id { get; set; }
        [StringLength(85)]
        public string Name { get; set; }
        [StringLength(85)]
        public string NormalizedName { get; set; }
        [StringLength(85)]
        public string ConcurrencyStamp { get; set; }
    }
}
