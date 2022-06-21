using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Settings
{
    public class FileModel : BaseModel
    {
        public string Filepath { get; set; }
        public string Filetype { get; set; }

        public int Downloadedcount { get; set; }

        public string Lastdownloadeduser { get; set; }

        public string Lastdownloadedip { get; set; }
    
    }
}
