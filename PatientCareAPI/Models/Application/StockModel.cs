using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Application
{
    public class StockModel : BaseModel
    {
        public int Order { get; set; }
        public string Stockid { get; set; }
        [NotMapped]
        public StockdefineModel Stockdefine { get; set; }
        public string Departmentid { get; set; }
        [NotMapped]
        public DepartmentModel Department { get; set; }
        public DateTime? Skt { get; set; }
        public string Barcodeno { get; set; }
        public double Usageamount { get; set; }
        public double Amount { get; set; }
        public double Maxamount { get; set; }
        public bool Isonusage { get; set; }
        public bool Isdeactive { get; set; }
        public DateTime? Deactivetime { get; set; }
        public string Info { get; set; }
        public string Source { get; set; }
        public string UnitID { get; set; }
        [NotMapped]
        public UnitModel Unit { get; set; }
    }
}
