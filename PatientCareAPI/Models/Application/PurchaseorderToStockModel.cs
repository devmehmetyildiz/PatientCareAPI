using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Application
{
    public class PurchaseorderToStockModel 
    {
        [Key]
        public int Id { get; set; }
        public string PurchaseID { get; set; }
        public string StockID { get; set; }
    }
}
