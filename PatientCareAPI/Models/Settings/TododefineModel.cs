using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Settings
{
    public class TododefineModel : BaseModel
    {
        public string TodogroupID { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public bool IsRequired { get; set; }
        public bool IsNeedactivation { get; set; }
    }
}
