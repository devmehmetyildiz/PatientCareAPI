﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Authentication
{
    public class UsertoSaltModel
    {
        [Key]
        public int Id { get; set; }
        [StringLength(85)]
        public string UserID { get; set; }

        [StringLength(85)]
        public string Salt { get; set; }
    }
}
