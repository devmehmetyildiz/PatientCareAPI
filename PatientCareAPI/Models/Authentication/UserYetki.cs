using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Authentication
{
    public class UserYetki
    {
        public const string Basic = "Basic";
        public const string User = "User";
        public const string Admin = "Admin";

        public const string User_Screen = "User_Screen";
        public const string User_Add = "Add_User";
        public const string User_Update = "User_Update";
        public const string User_Delete = "User_Delete";
      
        
    }
}
