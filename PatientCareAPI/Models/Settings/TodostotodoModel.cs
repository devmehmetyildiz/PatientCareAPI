﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Settings
{
    public class TodostotodoModel
    {
        [Key]
        public int Id { get; set; }
        public string TodosID { get; set; }
        public string TodoID { get; set; }
    }
}
