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
            Yetkis = new List<YetkiModel>();
        }
        [NotMapped]
        public List<YetkiModel> Yetkis{ get; set; }
    }
}
