using PatientCareAPI.Models.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Warehouse
{
    public class StockmovementModel : MovementbaseModel
    {
        [NotMapped]
        public StockModel Stock { get; set; }
    }
}
