using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PatientCareAPI.Models.Settings;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientCareAPI.Models.Authentication
{
    public class UsersModel : BaseModel
    {
        public UsersModel()
        {
            Stations = new List<StationsModel>();
            Departments = new List<DepartmentModel>();
            Roles = new List<RoleModel>();
            Files = new List<FileModel>();
        }
      
        [StringLength(85)]
        public string Username { get; set; }
        [StringLength(85)]
        public string NormalizedUsername { get; set; }
        [StringLength(85)]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        [StringLength(85)]
        public string PasswordHash { get; set; }
        public int AccessFailedCount { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [StringLength(85)]
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        [StringLength(85)]
        public string Town { get; set; }
        [StringLength(85)]
        public string City { get; set; }
        public string Address { get; set; }
        [StringLength(85)]
        public string Language { get; set; }
        public int UserID { get; set; }
        [NotMapped]
        public List<RoleModel> Roles { get; set; }
        public string Defaultdepartment { get; set; }
        [NotMapped]
        public List<DepartmentModel> Departments { get; set; }
        [NotMapped]
        public List<StationsModel> Stations { get; set; }
        [NotMapped]
        public List<FileModel> Files { get; set; }
    }
}
