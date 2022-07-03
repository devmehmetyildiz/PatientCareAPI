using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Application
{
    public class ProcessModel : BaseModel
    {
        public ProcessModel()
        {
            Activestocks = new List<ActivestockModel>();
            Users = new List<UsersModel>();
            Files = new List<FileModel>();
        }
        [NotMapped]
        public List<ActivestockModel> Activestocks { get; set; }
        [NotMapped]
        public List<UsersModel> Users { get; set; }
        [NotMapped]
        public List<FileModel> Files { get; set; }
        public string CaseId { get; set; }
        [NotMapped]
        public CaseModel Case { get; set; }
    }
}
