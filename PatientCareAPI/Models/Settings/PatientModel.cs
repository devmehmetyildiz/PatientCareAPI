﻿using PatientCareAPI.Models.Application;
using PatientCareAPI.Models.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Settings
{
    public class PatientModel : BaseModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        [StringLength(maximumLength:11)]
        public string CountryID { get; set; }
        public string Patienttypeid { get; set; }
        [NotMapped]
        public string Patienttypetxt { get; set; }
        [NotMapped]
        public PatienttypeModel Patienttype { get; set; }
    }
}
