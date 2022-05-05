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
        public string Name { get; set; }
        [StringLength(85)]
        public string NormalizedName { get; set; }
        [StringLength(85)]
        public string ConcurrencyStamp { get; set; }

        public string CreatedUser { get; set; }

        public string UpdatedUser { get; set; }

        public string DeleteUser { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public DateTime DeleteTime { get; set; }

        public bool IsActive { get; set; }
    }
}
