using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Authentication
{
    public class TablemetaconfigModel 
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Meta { get; set; }
        public string Config { get; set; }
    }
}
