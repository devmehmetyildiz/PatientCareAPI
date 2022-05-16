using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Authentication
{
    public class AuthoryModel : BaseModel
    {
        public AuthoryModel()
        {
            Roles = new List<RolesModel>();
        }
        [NotMapped]
        public List<RolesModel> Roles{ get; set; }
    }
}
