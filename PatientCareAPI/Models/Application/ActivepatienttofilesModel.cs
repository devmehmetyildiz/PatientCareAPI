using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Application
{
    public class ActivepatienttofilesModel
    {
        [Key]
        public int Id { get; set; }
        public string ActivepatientID { get; set; }
        public string ReportID { get; set; }
        public string FilesID { get; set; }
    }
}
