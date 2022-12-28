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
    public class PatientModel : BaseModel
    {
        public string PatientdefineID { get; set; }
        [NotMapped]
        public PatientdefineModel Patientdefine { get; set; }
        [NotMapped]
        public List<StockModel> Stocks { get; set; }
        [NotMapped]
        public int Patientstatus { get; set; }
        public List<FileModel> Files { get; set; }
        public DateTime? Approvaldate { get; set; }
        public DateTime? Registerdate { get; set; }
        public DateTime? Releasedate { get; set; }
        public int Roomnumber { get; set; }
        public int Floornumber { get; set; }
        public int Bednumber { get; set; }
        public string Departmentname { get; set; }
        public string Departmentid { get; set; }
        public bool Iswaitingactivation { get; set; }
        public string ImageID { get; set; }
        [NotMapped]
        public DepartmentModel Department { get; set; }
        public string CaseId { get; set; }
        [NotMapped]
        public CaseModel Case { get; set; }
    }
}
