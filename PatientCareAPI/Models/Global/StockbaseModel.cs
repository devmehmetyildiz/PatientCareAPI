using PatientCareAPI.Models.Settings;
using PatientCareAPI.Models.Warehouse;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Global
{
    public class StockbaseModel : BaseModel
    {
        public string StockdefineID { get; set; }
        [NotMapped]
        public StockdefineModel Stockdefine { get; set; }
        public string Departmentid { get; set; }
        [NotMapped]
        public DepartmentModel Department { get; set; }
        public DateTime? Skt { get; set; }
        public string Barcodeno { get; set; }
        [NotMapped]
        public double Amount { get; set; }
        public string Info { get; set; }
        public bool Willdelete { get; set; }
        public int Status { get; set; }
        public int Order { get; set; }
    }
}
