using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.System
{
    public class MailsettingModel : BaseModel
    {
        public string Name { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Smtphost { get; set; }
        public string Smtpport { get; set; }
        public string Mailaddress { get; set; }
        public bool Isbodyhtml { get; set; }
        public bool Issettingactive { get; set; }
    }
}
