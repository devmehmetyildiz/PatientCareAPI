using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Authentication
{
    public class NotificationModel : BaseModel
    {
        public string UserID { get; set; }
        public int Type { get; set; }
        public string SendeduserID { get; set; }
        public UsersModel Sendeduser { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public string Response { get; set; }
    }
}
