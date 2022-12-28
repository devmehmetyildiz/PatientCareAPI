﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Application
{
    public class StockmovementModel
    {
        [Key]
        public int Id { get; set; }
        [NotMapped]
        public StockModel Stock { get; set; }
        [NotMapped]
        public string Username { get; set; }
        [NotMapped]
        public string Movementtypename { get; set; }
        public string UserID { get; set; }
        public string Activestockid { get; set; }
        public int Movementtype { get; set; }
        public double Amount { get; set; }
        public double Prevvalue { get; set; }
        public double Newvalue { get; set; }
        public DateTime? Movementdate { get; set; }
    
    }
}
