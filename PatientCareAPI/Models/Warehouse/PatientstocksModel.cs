using PatientCareAPI.Models.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Warehouse
{
    public class PatientstocksModel : StockbaseModel
    {
        public string PatientID { get; set; }
    }
}
