using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Application
{
    public class PurchaseorderModel : BaseModel
    {
        [NotMapped]
        public List<StockModel> Stocks { get; set; }
        public string Info { get; set; }
        public string Company { get; set; }
        public string Username { get; set; }
        public double Purchaseprice { get; set; }
        public string Purchasenumber { get; set; }
        public string Companypersonelname { get; set; }
        public string Personelname { get; set; }
        public DateTime? Purchasedate { get; set; }
        public string CaseID { get; set; }
        [NotMapped]
        public CaseModel Case { get; set; }
    }
}
