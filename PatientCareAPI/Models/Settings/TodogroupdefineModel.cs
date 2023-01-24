using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Settings
{
    public class TodogroupdefineModel : BaseModel
    {
        public string Name { get; set; }
        public string DepartmentID { get; set; }
        [NotMapped]
        public List<TododefineModel> Todos { get; set; }
        [NotMapped]
        public DepartmentModel Department { get; set; }
    }
}
