using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Global
{
    public class MovementbaseModel : BaseModel
    {
        [NotMapped]
        public string Movementtypename { get; set; }
        public string StockID { get; set; }
        public int Movementtype { get; set; }
        public double Amount { get; set; }
        public double Prevvalue { get; set; }
        public double Newvalue { get; set; }
        public DateTime? Movementdate { get; set; }
        public int Status { get; set; }
    }
}
