using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Application
{
    public class PatientToStockModel
    {
        [Key]
        public int Id { get; set; }
        [StringLength(85)]
        public string PatientID { get; set; }
        [StringLength(85)]
        public string StockID { get; set; }
        public string ParentstockID { get; set; }
        public double Creatingamount { get; set; }
    }
}
