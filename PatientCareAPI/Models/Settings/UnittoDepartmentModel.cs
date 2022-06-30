using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Settings
{
    public class UnittoDepartmentModel
    {
        [Key]
        public int Id { get; set; }
        [StringLength(30)]
        public string UnitId { get; set; }
        [StringLength(30)]
        public string DepartmentId { get; set; }
    }
}
