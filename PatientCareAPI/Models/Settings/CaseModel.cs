using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Settings
{
    public class CaseModel : BaseModel
    {
        public string CaseGroup { get; set; }

        public int CaseStatus { get; set; }
    }
}
