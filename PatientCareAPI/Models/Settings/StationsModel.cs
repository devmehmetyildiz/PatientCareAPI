using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Settings
{
    public class StationsModel : BaseModel
    {
        public string Name { get; set; }
        [NotMapped]
        public bool isAdded { get; set; }

    }
}
