using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Settings
{
    public class CostumertypeModel : BaseModel
    {
        public CostumertypeModel()
        {
            Departments = new List<DepartmentModel>();
        }
        public string Name { get; set; }
        [NotMapped]
        public List<DepartmentModel> Departments { get; set; }
    }
}
