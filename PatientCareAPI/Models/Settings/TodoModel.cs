﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Settings
{
    public class TodoModel : BaseModel
    {
        public string Todoname { get; set; }
        public bool IsRequired { get; set; }
    }
}