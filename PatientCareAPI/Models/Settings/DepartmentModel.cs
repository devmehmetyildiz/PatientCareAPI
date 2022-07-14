using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Settings
{
    public class DepartmentModel : BaseModel
    {
        public DepartmentModel()
        {
            Stations = new List<StationsModel>();
        }

        public string Name { get; set; }

        [NotMapped]
        public bool IsAdded { get; set; }

        [NotMapped]
        public string Stationstxt { get; set; }

        [NotMapped]
        public List<StationsModel> Stations { get; set; }
    }
}
