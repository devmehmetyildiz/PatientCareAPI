using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Settings
{
    public class FileModel : BaseModel
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public string Parentid { get; set; }
        public string Filename { get; set; }
        public string Filefolder { get; set; }
        public string Filepath { get; set; }
        public string Filetype { get; set; }
        public string Usagetype { get; set; }
        public bool Canteditfile { get; set; }
        [NotMapped]
        public bool WillDelete { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
    }
}
