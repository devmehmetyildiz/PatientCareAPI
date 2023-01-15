using PatientCareAPI.Models.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Warehouse
{
    public class PatientstocksmovementModel : MovementbaseModel
    {
        [NotMapped]
        public PatientstocksModel Stock { get; set; }
    }
}
