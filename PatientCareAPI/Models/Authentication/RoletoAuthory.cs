﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Authentication
{
    public class RoletoAuthory
    {
        [Key]
        public int Id { get; set; }
        [StringLength(85)]
        public string RoleID { get; set; }
        [StringLength(85)]
        public string AuthoryID { get; set; }
    }
}
