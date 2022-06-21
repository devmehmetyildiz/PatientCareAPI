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
    public class ActivestockModel
    {
        [Key]
        public int Id { get; set; }

        public StockModel Stock { get; set; }
        public double amount { get; set; }
        public string UnitID { get; set; }
        public int MyProperty { get; set; }
    }
}
