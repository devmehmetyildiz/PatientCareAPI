using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }
        [StringLength(85)]
        public string ConcurrencyStamp { get; set; }
        [StringLength(30)]
        public string CreatedUser { get; set; }
        [StringLength(30)]
        public string UpdatedUser { get; set; }
        [StringLength(30)]
        public string DeleteUser { get; set; }
      
        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public DateTime? DeleteTime { get; set; }

        public bool IsActive { get; set; }
    }
}
