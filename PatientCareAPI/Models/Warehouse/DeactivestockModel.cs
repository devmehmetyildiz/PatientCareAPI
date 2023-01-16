using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Warehouse
{
    public class DeactivestockModel
    {
        [Key]
        public int Id { get; set; }
        public string DepartmentID { get; set; }
        [NotMapped]
        public DepartmentModel Department { get; set; }
        [NotMapped]
        public StockModel Stock { get; set; }
        public string StockID { get; set; }
        public DateTime? Createtime { get; set; }
        public string Createduser { get; set; }
        public double Amount { get; set; }
        public string Info { get; set; }
    }
}
