using PatientCareAPI.Models.Global;
using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Warehouse
{
    public class PurchaseorderstocksModel : StockbaseModel
    {
        public string PurchaseorderID { get; set; }
        [NotMapped]
        public PurchaseorderModel Purchaseorder { get; set; }
    }
}
