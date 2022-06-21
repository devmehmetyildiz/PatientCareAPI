using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Settings
{
    public class CasetoDepartmentModel
    {
        [Key]
        public int Id { get; set; }
        public string CaseID { get; set; }
        public string DepartmentID { get; set; }
    }
}
