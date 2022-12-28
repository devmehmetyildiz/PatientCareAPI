using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Global
{
    public class PasswordModel
    {
        public string Username { get; set; }
        public string Oldpassword { get; set; }
        public string Newpassword { get; set; }
    }
}
