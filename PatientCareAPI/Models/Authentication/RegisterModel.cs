using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Authentication
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Kullanıcı adı gereklidir")]
        public string Username { get; set; }
        [Required(ErrorMessage = "E-posta gereklidir.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Parola gereklidir.")]
        public string Password { get; set; }
    }
}
