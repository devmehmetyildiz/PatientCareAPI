using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Settings
{
    public class CostumertypetoDepartmentModel
    {
        [Key]
        public int Id { get; set; }
        public string CostumertypeID { get; set; }
        public string DepartmentID { get; set; }
    }
}
