using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Authentication
{
    public class RoleModel : BaseModel
    {
        public RoleModel()
        {
            Authories = new List<AuthoryModel>();
        }
        [NotMapped]
        public List<AuthoryModel> Authories { get; set; }
    }
}
