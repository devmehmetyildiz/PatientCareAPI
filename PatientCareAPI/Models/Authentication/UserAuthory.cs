using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientCareAPI.Models.Authentication
{
    public class UserAuthory
    {

        public const string AdminGroup = "Admin";
        public const string Roles = "Roles";
        public const string Department = "Department";
        public const string Station = "Station";
        public const string User = "User";
        public const string Case = "Case";
        public const string Unit = "Unit";
        public const string Stockdefine = "Stockdefine";
        public const string Files = "Files";
        public const string Costumertype = "Costumertype";
        public const string Patinettype = "Patinettype";
        public const string Tododefine = "Tododefine";
        public const string Todogroupdefine = "Todogroupdefine";
        public const string Checkperiod = "Checkperiod";
        public const string Period = "Period";
        public const string Mailsetting = "Mailsetting";
        public const string Printtemplate = "Printtemplate";
        public const string Warehouse = "Warehouse";
        public const string Stock = "Stock";
        public const string Stockmovement = "Stockmovement";
        public const string Purchaseorder = "Purchaseorder";
        public const string Purchaseorderstock = "Purchaseorderstock";
        public const string Purchaseorderstockmovement = "Purchaseorderstockmovement";
        public const string Preregistration = "Preregistration";
        public const string Patients = "Patients";
        public const string Patientstock = "Patientstock";
        public const string Patientstockmovement = "Patientstockmovement";
        public const string Patientdefine = "Patientdefine";

        public const string Admin = "Admin";

        public const string Roles_Screen = "Roles_Screen";
        public const string Roles_Getselected = "Roles_Getselected";
        public const string Roles_Add = "Roles_Add";
        public const string Roles_Edit = "Roles_Edit";
        public const string Roles_Delete = "Roles_Delete";
        public const string Roles_Getreport = "Roles_Getreport";
        public const string Roles_Columnchange = "Roles_Columnchange";

        public const string Department_Screen = "Department_Screen";
        public const string Department_Getselected = "Department_Getselected";
        public const string Department_Add = "Department_Add";
        public const string Department_Edit = "Department_Edit";
        public const string Department_Delete = "Department_Delete";
        public const string Department_Getreport = "Department_Getreport";
        public const string Department_Columnchange = "Department_Columnchange";

        public const string Station_Screen = "Station_Screen";
        public const string Station_Getselected = "Station_Getselected";
        public const string Station_Add = "Station_Add";
        public const string Station_Edit = "Station_Edit";
        public const string Station_Delete = "Station_Delete";
        public const string Station_Getreport = "Station_Getreport";
        public const string Station_Columnchange = "Station_Columnchange";

        public const string User_Screen = "User_Screen";
        public const string User_Getselected = "User_Getselected";
        public const string User_Add = "User_Add";
        public const string User_Edit = "User_Edit";
        public const string User_Delete = "User_Delete";
        public const string User_Getreport = "User_Getreport";
        public const string User_Columnchange = "User_Columnchange";

        public const string Case_Screen = "Case_Screen";
        public const string Case_Getselected = "Case_Getselected";
        public const string Case_Add = "Case_Add";
        public const string Case_Edit = "Case_Edit";
        public const string Case_Delete = "Case_Delete";
        public const string Case_Getreport = "Case_Getreport";
        public const string Case_Columnchange = "Case_Columnchange";

        public const string Unit_Screen = "Unit_Screen";
        public const string Unit_Getselected = "Unit_Getselected";
        public const string Unit_Add = "Unit_Add";
        public const string Unit_Edit = "Unit_Edit";
        public const string Unit_Delete = "Unit_Delete";
        public const string Unit_Getreport = "Unit_Getreport";
        public const string Unit_Columnchange = "Unit_Columnchange";

        public const string Stockdefine_Screen = "Stockdefine_Screen";
        public const string Stockdefine_Getselected = "Stockdefine_Getselected";
        public const string Stockdefine_Add = "Stockdefine_Add";
        public const string Stockdefine_Edit = "Stockdefine_Edit";
        public const string Stockdefine_Delete = "Stockdefine_Delete";
        public const string Stockdefine_Getreport = "Stockdefine_Getreport";
        public const string Stockdefine_Columnchange = "Stockdefine_Columnchange";

        public const string Files_Screen = "Files_Screen";
        public const string Files_Getselected = "Files_Getselected";
        public const string Files_Add = "Files_Add";
        public const string Files_Edit = "Files_Edit";
        public const string Files_Delete = "Files_Delete";
        public const string Files_Getreport = "Files_Getreport";
        public const string Files_Columnchange = "Files_Columnchange";

        public const string Costumertype_Screen = "Costumertype_Screen";
        public const string Costumertype_Getselected = "Costumertype_Getselected";
        public const string Costumertype_Add = "Costumertype_Add";
        public const string Costumertype_Edit = "Costumertype_Edit";
        public const string Costumertype_Delete = "Costumertype_Delete";
        public const string Costumertype_Getreport = "Costumertype_Getreport";
        public const string Costumertype_Columnchange = "Costumertype_Columnchange";

        public const string Patienttype_Screen = "Patienttype_Screen";
        public const string Patienttype_Getselected = "Patienttype_Getselected";
        public const string Patienttype_Add = "Patienttype_Add";
        public const string Patienttype_Edit = "Patienttype_Edit";
        public const string Patienttype_Delete = "Patienttype_Delete";
        public const string Patienttype_Getreport = "Patienttype_Getreport";
        public const string Patienttype_Columnchange = "Patienttype_Columnchange";

        public const string Tododefine_Screen = "Tododefine_Screen";
        public const string Tododefine_Getselected = "Tododefine_Getselected";
        public const string Tododefine_Add = "Tododefine_Add";
        public const string Tododefine_Edit = "Tododefine_Edit";
        public const string Tododefine_Delete = "Tododefine_Delete";
        public const string Tododefine_Getreport = "Tododefine_Getreport";
        public const string Tododefine_Columnchange = "Tododefine_Columnchange";

        public const string Todogroupdefine_Screen = "Todogroupdefine_Screen";
        public const string Todogroupdefine_Getselected = "Todogroupdefine_Getselected";
        public const string Todogroupdefine_Add = "Todogroupdefine_Add";
        public const string Todogroupdefine_Edit = "Todogroupdefine_Edit";
        public const string Todogroupdefine_Delete = "Todogroupdefine_Delete";
        public const string Todogroupdefine_Getreport = "Todogroupdefine_Getreport";
        public const string Todogroupdefine_Columnchange = "Todogroupdefine_Columnchange";

        public const string Checkperiod_Screen = "Checkperiod_Screen";
        public const string Checkperiod_Getselected = "Checkperiod_Getselected";
        public const string Checkperiod_Add = "Checkperiod_Add";
        public const string Checkperiod_Edit = "Checkperiod_Edit";
        public const string Checkperiod_Delete = "Checkperiod_Delete";
        public const string Checkperiod_Getreport = "Checkperiod_Getreport";
        public const string Checkperiod_Columnchange = "Checkperiod_Columnchange";

        public const string Period_Screen = "Period_Screen";
        public const string Period_Getselected = "Period_Getselected";
        public const string Period_Add = "Period_Add";
        public const string Period_Edit = "Period_Edit";
        public const string Period_Delete = "Period_Delete";
        public const string Period_Getreport = "Period_Getreport";
        public const string Period_Columnchange = "Period_Columnchange";

        public const string Mailsetting_Screen = "Mailsetting_Screen";
        public const string Mailsetting_Getselected = "Mailsetting_Getselected";
        public const string Mailsetting_Add = "Mailsetting_Add";
        public const string Mailsetting_Edit = "Mailsetting_Edit";
        public const string Mailsetting_Delete = "Mailsetting_Delete";
        public const string Mailsetting_Getreport = "Mailsetting_Getreport";
        public const string Mailsetting_Columnchange = "Mailsetting_Columnchange";

        public const string Printtemplate_Screen = "Printtemplate_Screen";
        public const string Printtemplate_Getselected = "Printtemplate_Getselected";
        public const string Printtemplate_Add = "Printtemplate_Add";
        public const string Printtemplate_Edit = "Printtemplate_Edit";
        public const string Printtemplate_Delete = "Printtemplate_Delete";
        public const string Printtemplate_Getreport = "Printtemplate_Getreport";
        public const string Printtemplate_Columnchange = "Printtemplate_Columnchange";

        public const string Warehouse_Screen = "Warehouse_Screen";
        public const string Warehouse_Getselected = "Warehouse_Getselected";
        public const string Warehouse_Add = "Warehouse_Add";
        public const string Warehouse_Edit = "Warehouse_Edit";
        public const string Warehouse_Delete = "Warehouse_Delete";
        public const string Warehouse_Getreport = "Warehouse_Getreport";
        public const string Warehouse_Columnchange = "Warehouse_Columnchange";

        public const string Stock_Screen = "Stock_Screen";
        public const string Stock_Getselected = "Stock_Getselected";
        public const string Stock_Add = "Stock_Add";
        public const string Stock_Edit = "Stock_Edit";
        public const string Stock_Delete = "Stock_Delete";
        public const string Stock_Getreport = "Stock_Getreport";
        public const string Stock_Columnchange = "Stock_Columnchange";

        public const string Stockmovement_Screen = "Stockmovement_Screen";
        public const string Stockmovement_Getselected = "Stockmovement_Getselected";
        public const string Stockmovement_Add = "Stockmovement_Add";
        public const string Stockmovement_Edit = "Stockmovement_Edit";
        public const string Stockmovement_Delete = "Stockmovement_Delete";
        public const string Stockmovement_Getreport = "Stockmovement_Getreport";
        public const string Stockmovement_Columnchange = "Stockmovement_Columnchange";

        public const string Purchaseorder_Screen = "Purchaseorder_Screen";
        public const string Purchaseorder_Getselected = "Purchaseorder_Getselected";
        public const string Purchaseorder_Add = "Purchaseorder_Add";
        public const string Purchaseorder_Edit = "Purchaseorder_Edit";
        public const string Purchaseorder_Delete = "Purchaseorder_Delete";
        public const string Purchaseorder_Getreport = "Purchaseorder_Getreport";
        public const string Purchaseorder_Columnchange = "Purchaseorder_Columnchange";

        public const string Purchaseorderstock_Screen = "Purchaseorderstock_Screen";
        public const string Purchaseorderstock_Getselected = "Purchaseorderstock_Getselected";
        public const string Purchaseorderstock_Add = "Purchaseorderstock_Add";
        public const string Purchaseorderstock_Edit = "Purchaseorderstock_Edit";
        public const string Purchaseorderstock_Delete = "Purchaseorderstock_Delete";
        public const string Purchaseorderstock_Getreport = "Purchaseorderstock_Getreport";
        public const string Purchaseorderstock_Columnchange = "Purchaseorderstock_Columnchange";

        public const string Purchaseorderstockmovement_Screen = "Purchaseorderstockmovement_Screen";
        public const string Purchaseorderstockmovement_Getselected = "Purchaseorderstockmovement_Getselected";
        public const string Purchaseorderstockmovement_Add = "Purchaseorderstockmovement_Add";
        public const string Purchaseorderstockmovement_Edit = "Purchaseorderstockmovement_Edit";
        public const string Purchaseorderstockmovement_Delete = "Purchaseorderstockmovement_Delete";
        public const string Purchaseorderstockmovement_Getreport = "Purchaseorderstockmovement_Getreport";
        public const string Purchaseorderstockmovement_Columnchange = "Purchaseorderstockmovement_Columnchange";

        public const string Preregistration_Screen = "Preregistration_Screen";
        public const string Preregistration_Getselected = "Preregistration_Getselected";
        public const string Preregistration_Add = "Preregistration_Add";
        public const string Preregistration_Edit = "Preregistration_Edit";
        public const string Preregistration_Delete = "Preregistration_Delete";
        public const string Preregistration_Getreport = "Preregistration_Getreport";
        public const string Preregistration_Columnchange = "Preregistration_Columnchange";

        public const string Patients_Screen = "Patients_Screen";
        public const string Patients_Getselected = "Patients_Getselected";
        public const string Patients_Add = "Patients_Add";
        public const string Patients_Edit = "Patients_Edit";
        public const string Patients_Delete = "Patients_Delete";
        public const string Patients_Getreport = "Patients_Getreport";
        public const string Patients_Columnchange = "Patients_Columnchange";

        public const string Patientstock_Screen = "Patientstock_Screen";
        public const string Patientstock_Getselected = "Patientstock_Getselected";
        public const string Patientstock_Add = "Patientstock_Add";
        public const string Patientstock_Edit = "Patientstock_Edit";
        public const string Patientstock_Delete = "Patientstock_Delete";
        public const string Patientstock_Getreport = "Patientstock_Getreport";
        public const string Patientstock_Columnchange = "Patientstock_Columnchange";

        public const string Patientstockmovement_Screen = "Patientstockmovement_Screen";
        public const string Patientstockmovement_Getselected = "Patientstockmovement_Getselected";
        public const string Patientstockmovement_Add = "Patientstockmovement_Add";
        public const string Patientstockmovement_Edit = "Patientstockmovement_Edit";
        public const string Patientstockmovement_Delete = "Patientstockmovement_Delete";
        public const string Patientstockmovement_Getreport = "Patientstockmovement_Getreport";
        public const string Patientstockmovement_Columnchange = "Patientstockmovement_Columnchange";

        public const string Patientdefine_Screen = "Patientdefine_Screen";
        public const string Patientdefine_Getselected = "Patientdefine_Getselected";
        public const string Patientdefine_Add = "Patientdefine_Add";
        public const string Patientdefine_Edit = "Patientdefine_Edit";
        public const string Patientdefine_Delete = "Patientdefine_Delete";
        public const string Patientdefine_Getreport = "Patientdefine_Getreport";
        public const string Patientdefine_Columnchange = "Patientdefine_Columnchange";

    }
}
