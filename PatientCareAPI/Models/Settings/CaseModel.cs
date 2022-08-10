using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Settings
{
    public class CaseModel : BaseModel
    {
        public CaseModel()
        {
            Departments = new List<DepartmentModel>();
        }
        public int CaseStatus { get; set; }
        public string  Name { get; set; }
        public string Shortname { get; set; }
        [NotMapped]
        public string Departmentstxt { get; set; }

        [NotMapped]
        public List<DepartmentModel> Departments { get; set; }
    }
}
