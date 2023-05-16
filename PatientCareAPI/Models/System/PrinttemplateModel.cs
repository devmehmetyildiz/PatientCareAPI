using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.System
{
    public class PrinttemplateModel : BaseModel
    {
        public string Name { get; set; }
        public string Printtemplate { get; set; }
        public string Valuekey { get; set; }
        public string DepartmentID { get; set; }
        [NotMapped]
        public DepartmentModel Department { get; set; }
    }
}
