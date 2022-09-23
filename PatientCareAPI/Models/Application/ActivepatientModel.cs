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
    public class ActivepatientModel : BaseModel
    {
        public string PatientID { get; set; }
        [NotMapped]
        public PatientModel Patient { get; set; }
        [NotMapped]
        public PatientapplicantModel Applicant { get; set; }
        [NotMapped]
        public List<PatientbodycontrolformModel> Bodycontrolforms { get; set; }
        [NotMapped]
        public List<PatientdiagnosisModel> Diagnosis { get; set; }
        [NotMapped]
        public List<PatientdisabilitypermitformModel> Disabilitypermitforms { get; set; }
        [NotMapped]
        public List<PatientdisabledhealthboardreportModel> Disabledhealthboardreports { get; set; }
        [NotMapped]
        public List<PatientfirstadmissionformModel> Firstadmissionforms { get; set; }
        [NotMapped]
        public List<PatientfirstapproachreportModel> Firstapproachreports { get; set; }
        [NotMapped]
        public List<PatientownershiprecieveModel> Ownershiprecieves { get; set; }
        [NotMapped]
        public List<PatientrecieveformModel> Recieveforms { get; set; }
        [NotMapped]
        public List<PatientsubmittingformModel> Submittingforms { get; set; }
        [NotMapped]
        public List<ActivestockModel> Stocks { get; set; }
        [NotMapped]
        public List<FileModel> Files { get; set; }
        public DateTime? Approvaldate { get; set; }
        public DateTime? Registerdate { get; set; }
        public string Patientdiagnosis { get; set; }
        public DateTime? Releasedate { get; set; }
        public int Roomnumber { get; set; }
        public int Floornumber { get; set; }
        public int Bednumber { get; set; }
        public string Departmentname { get; set; }
        public string Departmentid { get; set; }
        public string Processid { get; set; }
        public bool Iswaitingactivation { get; set; }
        public string ImageID { get; set; }
        [NotMapped]
        public DepartmentModel Department { get; set; }
        public string CaseId { get; set; }
        [NotMapped]
        public CaseModel Case { get; set; }
    }
}
