using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Global
{
    public class PatientformbaseModel : BaseModel
    {
        public string Documentcode { get; set; }
        public DateTime? Releasedate { get; set; }
        public DateTime? Revisiondate { get; set; }
        public DateTime? Actualdate { get; set; }
    }
}
