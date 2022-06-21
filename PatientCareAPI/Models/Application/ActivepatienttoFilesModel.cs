using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Application
{
    public class ActivepatienttoFilesModel
    {
        [Key]
        public int Id { get; set; }
        [StringLength(85)]
        public string ActivepatientID { get; set; }
        [StringLength(85)]
        public string FilesID { get; set; }
    }
}
