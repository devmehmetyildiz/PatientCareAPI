using PatientCareAPI.Models.Authentication;
using PatientCareAPI.Models.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Application
{
    public class ActivepatientModel
    {
        public ActivepatientModel()
        {
            Activestocks = new List<ActivestockModel>();
            Users = new List<UsersModel>();
            Files = new List<FileModel>();
        }
        [Key]
        public int Id { get; set; }
        [StringLength(85)]
        public string PatientID { get; set; }
        [NotMapped]
        public PatientModel Patient { get; set; }
        public DateTime? Registerdate { get; set; }
        public DateTime? Releasedate { get; set; }
        [NotMapped]
        public List<ActivestockModel> Activestocks { get; set; }
        [NotMapped]
        public List<UsersModel> Users { get; set; }
        [NotMapped]
        public List<FileModel> Files { get; set; }
    }
}
