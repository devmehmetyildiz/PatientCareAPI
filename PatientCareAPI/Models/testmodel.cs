using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models
{
    public class testmodel
    {
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
