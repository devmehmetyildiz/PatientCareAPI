using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Settings
{
    public class UnitModel : BaseModel
    {
        public UnitModel()
        {
            Departments = new List<DepartmentModel>();
        }
        [StringLength(30)]
        public string Name { get; set; }
        public int Unittype { get; set; }

        [NotMapped]
        public List<DepartmentModel> Departments { get; set; }
        
    }
}
