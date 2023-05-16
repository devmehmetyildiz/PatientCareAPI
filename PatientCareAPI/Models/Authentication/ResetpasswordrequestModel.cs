using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Authentication
{
    public class ResetpasswordrequestModel
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Hashkey { get; set; }
        [NotMapped]
        public string Newpassword { get; set; }
        public DateTime? Expiretime { get; set; }
        public bool IsActive { get; set; }

    }
}
