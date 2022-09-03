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
        public PatientbodycontrolformModel Bodycontrolform { get; set; }
        [NotMapped]
        public List<PatientdiagnosisModel> Diagnosis { get; set; }
        [NotMapped]
        public PatientdisabilitypermitformModel Disabilitypermitform { get; set; }
        [NotMapped]
        public PatientdisabledhealthboardreportModel Disabledhealthboardreport { get; set; }
        [NotMapped]
        public PatientfirstadmissionformModel Firstadmissionform { get; set; }
        [NotMapped]
        public PatientfirstapproachreportModel Firstapproachreport { get; set; }
        [NotMapped]
        public PatientownershiprecieveModel Ownershiprecieve { get; set; }
        [NotMapped]
        public PatientrecieveformModel Recieveform { get; set; }
        [NotMapped]
        public PatientsubmittingformModel Submittingform { get; set; }
        public DateTime? Approvaldate { get; set; }
        public DateTime? Registerdate { get; set; }
        public string Patientdiagnosis { get; set; }
        public DateTime? Releasedate { get; set; }
        public int Roomnumber { get; set; }
        public int Floornumber { get; set; }
        public int Bednumber { get; set; }
        public string Departmentname { get; set; }
        public string Departmentid { get; set; }
        [NotMapped]
        public DepartmentModel Department { get; set; }
        public string Processid { get; set; }
        public bool Iswaitingactivation { get; set; }
        [NotMapped]
        public ProcessModel Process { get; set; }
        public string ImageID { get; set; }
        public string CaseId { get; set; }
        [NotMapped]
        public CaseModel Case { get; set; }
    }
}
