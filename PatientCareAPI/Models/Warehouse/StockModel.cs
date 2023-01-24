using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Models.Global;
using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Warehouse
{
    public class StockModel : StockbaseModel
    {
        public string WarehouseID { get; set; }
        [NotMapped]
        public WarehouseModel Warehouse { get; set; }
        public bool Isonusage { get; set; }
        public bool Isdeactive { get; set; }
        public DateTime? Deactivetime { get; set; }
        public string Source { get; set; }
    }
}
