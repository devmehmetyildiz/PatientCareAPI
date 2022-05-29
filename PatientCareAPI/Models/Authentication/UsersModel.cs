using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Authentication
{
    public class UsersModelzz
    {
        [Key]
        public int Id { get; set; }

        [StringLength(85)]
        public string Username { get; set; }

        [StringLength(85)]
        public string NormalizedUsername { get; set; }

        [StringLength(85)]
        public string Email { get; set; }

        [StringLength(85)]
        public bool EmailConfirmed { get; set; }

        [StringLength(85)]
        public string PasswordHash { get; set; }

        [StringLength(85)]
        public string ConcurrencyStamp { get; set; }

        [StringLength(85)]
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool Isactive { get; set; }
        public int AccessFailedCount { get; set; }
    }
}
