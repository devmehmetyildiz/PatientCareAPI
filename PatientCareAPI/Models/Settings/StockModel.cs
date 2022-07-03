﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Settings
{
    public class StockModel : BaseModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Skt { get; set; }
        public string Unitid { get; set; }
        [NotMapped]
        public UnitModel Unit { get; set; }
        public string Departmentid { get; set; }
        [NotMapped]
        public DepartmentModel Department { get; set; }
    }
}
