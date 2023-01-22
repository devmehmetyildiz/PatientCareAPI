using PatientCareAPI.Models.Application;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Warehouse
{
    public class WarehouseModel : BaseModel
    {
        public string Name { get; set; }
        [NotMapped]
        public List<StockModel> Stocks { get; set; }
        public string Info { get; set; }
    }
}
