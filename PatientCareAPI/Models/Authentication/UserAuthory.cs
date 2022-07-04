using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Authentication
{
    public class UserAuthory
    {
        public const string Basic = "Basic";
        public const string User = "User";
        public const string Admin = "Admin";

        public const string User_Screen = "User_Screen";
        public const string User_Add = "Add_User";
        public const string User_Update = "User_Update";
        public const string User_Delete = "User_Delete";
        public const string User_ManageAll = "User_Delete";

        public const string Department_Screen = "Department_Screen";
        public const string Department_Add = "Department_Add";
        public const string Department_Update = "Department_Update";
        public const string Department_Delete = "Department_Delete";
        public const string Department_ManageAll = "Department_ManageAll";

        public const string Stock_Screen = "Stock_Screen";
        public const string Stock_Add = "Stock_Add";
        public const string Stock_Update = "Stock_Update";
        public const string Stock_Delete = "Stock_Delete";
        public const string Stock_ManageAll = "Stock_ManageAll";

        public const string Patients_Screen = "Patients_Screen";
        public const string Patients_Add = "Patients_Add";
        public const string Patients_Update = "Patients_Update";
        public const string Patients_Delete = "Patients_Delete";
        public const string Patients_ManageAll = "Patients_ManageAll";
        public const string Patients_UploadFile = "Patient_UploadFile";
        public const string Patients_DownloadFile = "Patient_DownloadFile";
        public const string Patients_ViewFile = "Patient_ViewFile";

        public const string Patienttype_Screen = "Patienttype_Screen";
        public const string Patienttype_Add = "Patienttype_Add";
        public const string Patienttype_Update = "Patienttype_Update";
        public const string Patienttype_Delete = "Patienttype_Delete";
        public const string Patienttype_ManageAll = "Patienttype_ManageAll";

        public const string Unit_Screen = "Unit_Screen";
        public const string Unit_Add = "Unit_Add";
        public const string Unit_Update = "Unit_Update";
        public const string Unit_Delete = "Unit_Delete";
        public const string Unit_ManageAll = "Unit_ManageAll";

        public const string Case_Screen = "Case_Screen";
        public const string Case_Add = "Case_Add";
        public const string Case_Update = "Case_Update";
        public const string Case_Delete = "Case_Delete";
        public const string Case_ManageAll = "Case_ManageAll";

        public const string Roles_Screen = "Roles_Screen";
        public const string Roles_Add = "Roles_Add";
        public const string Roles_Update = "Roles_Update";
        public const string Roles_Delete = "Roles_Delete";
        public const string Roles_ManageAll = "Roles_ManageAll";

        public const string File_Screen = "File_Screen";
        public const string File_Add = "File_Add";
        public const string File_Update = "File_Update";
        public const string File_Delete = "File_Delete";
        public const string File_ManageAll = "File_ManageAll";

        public const string Dashboard_AllScreen = "Dashboard_AllScreen";
        public const string Dashboard_DepartmentScreen = "Dashboard_DepartmentScreen";

        public const string Reminding_Screen = "Reminding_Screen";
        public const string Reminding_Add = "Reminding_Add";
        public const string Reminding_Update = "Reminding_Update";
        public const string Reminding_Delete = "Reminding_Delete";
        public const string Reminding_ManageAll = "Reminding_ManageAll";
        public const string Reminding_DefineforAll = "Reminding_DefineforAll";
        public const string Reminding_Define = "Reminding_Define";
    }
}
