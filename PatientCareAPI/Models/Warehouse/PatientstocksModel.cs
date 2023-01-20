using PatientCareAPI.Models.Application;
using PatientCareAPI.Models.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Warehouse
{
    public class PatientstocksModel : StockbaseModel
    {
        public string PatientID { get; set; }
        [NotMapped]
        public PatientModel Patient { get; set; }
    }
}
