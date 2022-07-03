using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Settings
{
    public class UsertoDepartmentModel
    {
        [Key]
        public int Id { get; set; }
        public string UserID { get; set; }

        public string DepartmanID { get; set; }
    }
}
