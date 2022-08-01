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
    public class ActivestockModel : BaseModel
    {
        [NotMapped]
        public string Stockname { get; set; }
        public string Stockid { get; set; }
        [NotMapped]
        public StockModel Stock { get; set; }
        [NotMapped]
        public string Departmentname { get; set; }
        public string Departmentid { get; set; }
        [NotMapped]
        public DepartmentModel Department { get; set; }
        public DateTime? Skt { get; set; }
        public string Barcodeno { get; set; }
        public double Amount { get; set; }
        public double Purchaseprice { get; set; }
        public DateTime? Purchasedate { get; set; }
        public string Info { get; set; }

    }
}
