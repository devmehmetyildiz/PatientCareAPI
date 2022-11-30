using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace PatientCareAPI.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activepatientmovementstotodos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Vieworder = table.Column<int>(type: "int", nullable: false),
                    MovementID = table.Column<string>(type: "text", nullable: true),
                    TodoID = table.Column<string>(type: "text", nullable: true),
                    Iscomplated = table.Column<string>(type: "text", nullable: true),
                    ComplatedUser = table.Column<string>(type: "text", nullable: true),
                    Complatetime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activepatientmovementstotodos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Activepatients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    PatientID = table.Column<string>(type: "text", nullable: true),
                    Approvaldate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Registerdate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Patientdiagnosis = table.Column<string>(type: "text", nullable: true),
                    Releasedate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Roomnumber = table.Column<int>(type: "int", nullable: false),
                    Floornumber = table.Column<int>(type: "int", nullable: false),
                    Bednumber = table.Column<int>(type: "int", nullable: false),
                    Departmentname = table.Column<string>(type: "text", nullable: true),
                    Departmentid = table.Column<string>(type: "text", nullable: true),
                    Processid = table.Column<string>(type: "text", nullable: true),
                    Iswaitingactivation = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ImageID = table.Column<string>(type: "text", nullable: true),
                    CaseId = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activepatients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActivepatientstoFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ActivepatientID = table.Column<string>(type: "text", nullable: true),
                    ReportID = table.Column<string>(type: "text", nullable: true),
                    FilesID = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivepatientstoFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActivepatienttoActivestocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ActivepatientID = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    ActivestocksID = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivepatienttoActivestocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Activestock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Stockid = table.Column<string>(type: "text", nullable: true),
                    Departmentid = table.Column<string>(type: "text", nullable: true),
                    Skt = table.Column<DateTime>(type: "datetime", nullable: true),
                    Barcodeno = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<double>(type: "double", nullable: false),
                    Purchaseprice = table.Column<double>(type: "double", nullable: false),
                    Purchasedate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Isdeactive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Deactivetime = table.Column<DateTime>(type: "datetime", nullable: true),
                    Info = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activestock", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Authories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    Group = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CaseStatus = table.Column<int>(type: "int", nullable: false),
                    Casecolor = table.Column<string>(type: "text", nullable: true),
                    Shortname = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CasetoDepartments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CaseID = table.Column<string>(type: "text", nullable: true),
                    DepartmentID = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CasetoDepartments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Costumertypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Costumertypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CostumertypetoDepartments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CostumertypeID = table.Column<string>(type: "text", nullable: true),
                    DepartmentID = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostumertypetoDepartments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Datatables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Tablename = table.Column<string>(type: "text", nullable: true),
                    Json = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Datatables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Deactivestocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Activestockid = table.Column<string>(type: "text", nullable: true),
                    Createtime = table.Column<DateTime>(type: "datetime", nullable: true),
                    Createduser = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deactivestocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Ishavepatients = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DepartmenttoStation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DepartmentID = table.Column<string>(type: "text", nullable: true),
                    StationID = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmenttoStation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Filename = table.Column<string>(type: "text", nullable: true),
                    Filefolder = table.Column<string>(type: "text", nullable: true),
                    Filepath = table.Column<string>(type: "text", nullable: true),
                    Filetype = table.Column<string>(type: "text", nullable: true),
                    Downloadedcount = table.Column<int>(type: "int", nullable: false),
                    Lastdownloadeduser = table.Column<string>(type: "text", nullable: true),
                    Lastdownloadedip = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patientactivestocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Activestockid = table.Column<string>(type: "text", nullable: true),
                    Createtime = table.Column<DateTime>(type: "datetime", nullable: true),
                    Createduser = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patientactivestocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patientapplicants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Activepatientid = table.Column<string>(type: "text", nullable: true),
                    Firstname = table.Column<string>(type: "text", nullable: true),
                    Lastname = table.Column<string>(type: "text", nullable: true),
                    Proximitystatus = table.Column<string>(type: "text", nullable: true),
                    Countryid = table.Column<string>(type: "text", nullable: true),
                    Fathername = table.Column<string>(type: "text", nullable: true),
                    Mothername = table.Column<string>(type: "text", nullable: true),
                    Dateofbirth = table.Column<DateTime>(type: "datetime", nullable: true),
                    Placeofbirth = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<string>(type: "text", nullable: true),
                    Marialstatus = table.Column<string>(type: "text", nullable: true),
                    Jobstatus = table.Column<string>(type: "text", nullable: true),
                    Educationstatus = table.Column<string>(type: "text", nullable: true),
                    Montlyincome = table.Column<string>(type: "text", nullable: true),
                    Town = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    Address1 = table.Column<string>(type: "text", nullable: true),
                    Address2 = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    Contactnumber1 = table.Column<string>(type: "text", nullable: true),
                    Contactnumber2 = table.Column<string>(type: "text", nullable: true),
                    Contactname1 = table.Column<string>(type: "text", nullable: true),
                    Contactname2 = table.Column<string>(type: "text", nullable: true),
                    Appialdate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Appialreason = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patientapplicants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patientbodycontrolforms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Activepatientid = table.Column<string>(type: "text", nullable: true),
                    Info = table.Column<string>(type: "text", nullable: true),
                    Checkreason = table.Column<string>(type: "text", nullable: true),
                    Controllername = table.Column<string>(type: "text", nullable: true),
                    Cotrollername1 = table.Column<string>(type: "text", nullable: true),
                    Controllername2 = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Documentcode = table.Column<string>(type: "text", nullable: true),
                    Releasedate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Revisiondate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Actualdate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patientbodycontrolforms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patientdiagnosis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Reportid = table.Column<string>(type: "text", nullable: true),
                    Diagnosisname = table.Column<string>(type: "text", nullable: true),
                    Diagnosisstatus = table.Column<string>(type: "text", nullable: true),
                    Info = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patientdiagnosis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patientdisabilitypermitforms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Activepatientid = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Documentcode = table.Column<string>(type: "text", nullable: true),
                    Releasedate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Revisiondate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Actualdate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patientdisabilitypermitforms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patientdisabledhealthboardreports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Activepatientid = table.Column<string>(type: "text", nullable: true),
                    Reportno = table.Column<string>(type: "text", nullable: true),
                    Reportname = table.Column<string>(type: "text", nullable: true),
                    Sendinginstitution = table.Column<string>(type: "text", nullable: true),
                    Appealdate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Disabilityname = table.Column<string>(type: "text", nullable: true),
                    Disabilityinfo = table.Column<string>(type: "text", nullable: true),
                    Disabilityrate = table.Column<string>(type: "text", nullable: true),
                    Disabilitystatus = table.Column<string>(type: "text", nullable: true),
                    Wontworkjobs = table.Column<string>(type: "text", nullable: true),
                    Ispermanent = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Documentcode = table.Column<string>(type: "text", nullable: true),
                    Releasedate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Revisiondate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Actualdate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patientdisabledhealthboardreports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patientfirstadmissionforms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ActivepatientID = table.Column<string>(type: "text", nullable: true),
                    Patienttype = table.Column<string>(type: "text", nullable: true),
                    Locationknowledge = table.Column<string>(type: "text", nullable: true),
                    Ishaveitem = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Itemstxt = table.Column<string>(type: "text", nullable: true),
                    Reportstatus = table.Column<string>(type: "text", nullable: true),
                    Reportvaliddate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Reportdegree = table.Column<string>(type: "text", nullable: true),
                    Bodycontroldate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Disableorientation = table.Column<string>(type: "text", nullable: true),
                    Controllername = table.Column<string>(type: "text", nullable: true),
                    Managername = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Documentcode = table.Column<string>(type: "text", nullable: true),
                    Releasedate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Revisiondate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Actualdate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patientfirstadmissionforms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patientfirstapproachreports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ActivepatientID = table.Column<string>(type: "text", nullable: true),
                    Acceptancedate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Interviewdate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Healthinitialassesmentdate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Reasonforhealtcare = table.Column<string>(type: "text", nullable: true),
                    Ratinginfo = table.Column<string>(type: "text", nullable: true),
                    Knowledgesource = table.Column<string>(type: "text", nullable: true),
                    Controllername = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Documentcode = table.Column<string>(type: "text", nullable: true),
                    Releasedate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Revisiondate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Actualdate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patientfirstapproachreports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patientmovements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Movementid = table.Column<string>(type: "text", nullable: true),
                    Activepatientid = table.Column<string>(type: "text", nullable: true),
                    Movementtype = table.Column<int>(type: "int", nullable: false),
                    Iswaitingactivation = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeactive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Oldstatus = table.Column<string>(type: "text", nullable: true),
                    NewStatus = table.Column<string>(type: "text", nullable: true),
                    IsTodoneeded = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsComplated = table.Column<string>(type: "text", nullable: true),
                    Movementdate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UserID = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patientmovements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patientownershiprecieves",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Activepatientid = table.Column<string>(type: "text", nullable: true),
                    Itemstxt = table.Column<string>(type: "text", nullable: true),
                    Recievername = table.Column<string>(type: "text", nullable: true),
                    Recievercountryno = table.Column<string>(type: "text", nullable: true),
                    Submittercountryno = table.Column<string>(type: "text", nullable: true),
                    Submittername = table.Column<string>(type: "text", nullable: true),
                    Witnessname = table.Column<string>(type: "text", nullable: true),
                    Witnesscountryid = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Documentcode = table.Column<string>(type: "text", nullable: true),
                    Releasedate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Revisiondate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Actualdate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patientownershiprecieves", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patientrecieveforms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Activepatientid = table.Column<string>(type: "text", nullable: true),
                    Reportdate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Itemstxt = table.Column<string>(type: "text", nullable: true),
                    Submittername = table.Column<string>(type: "text", nullable: true),
                    Submittercountryid = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Documentcode = table.Column<string>(type: "text", nullable: true),
                    Releasedate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Revisiondate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Actualdate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patientrecieveforms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Firstname = table.Column<string>(type: "text", nullable: true),
                    Lastname = table.Column<string>(type: "text", nullable: true),
                    Fathername = table.Column<string>(type: "text", nullable: true),
                    Mothername = table.Column<string>(type: "text", nullable: true),
                    Motherbiologicalaffinity = table.Column<string>(type: "text", nullable: true),
                    Ismotheralive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Fatherbiologicalaffinity = table.Column<string>(type: "text", nullable: true),
                    Isfatheralive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CountryID = table.Column<string>(type: "text", nullable: true),
                    Dateofbirth = table.Column<DateTime>(type: "datetime", nullable: true),
                    Placeofbirth = table.Column<string>(type: "text", nullable: true),
                    Dateofdeath = table.Column<DateTime>(type: "datetime", nullable: true),
                    Placeofdeath = table.Column<string>(type: "text", nullable: true),
                    Deathinfo = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<string>(type: "text", nullable: true),
                    Marialstatus = table.Column<string>(type: "text", nullable: true),
                    Criminalrecord = table.Column<string>(type: "text", nullable: true),
                    Childnumber = table.Column<int>(type: "int", nullable: false),
                    Disabledchildnumber = table.Column<int>(type: "int", nullable: false),
                    Siblingstatus = table.Column<string>(type: "text", nullable: true),
                    Sgkstatus = table.Column<string>(type: "text", nullable: true),
                    Budgetstatus = table.Column<string>(type: "text", nullable: true),
                    Town = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    Address1 = table.Column<string>(type: "text", nullable: true),
                    Address2 = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    Contactnumber1 = table.Column<string>(type: "text", nullable: true),
                    Contactnumber2 = table.Column<string>(type: "text", nullable: true),
                    Contactname1 = table.Column<string>(type: "text", nullable: true),
                    Contactname2 = table.Column<string>(type: "text", nullable: true),
                    Costumertypeid = table.Column<string>(type: "text", nullable: true),
                    Patienttypeid = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patientsubmittingforms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Activepatientid = table.Column<string>(type: "text", nullable: true),
                    Itemstxt = table.Column<string>(type: "text", nullable: true),
                    Submitterpersonelname = table.Column<string>(type: "text", nullable: true),
                    Recievername = table.Column<string>(type: "text", nullable: true),
                    Recievercountryno = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Documentcode = table.Column<string>(type: "text", nullable: true),
                    Releasedate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Revisiondate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Actualdate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patientsubmittingforms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patienttypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patienttypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Remindings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remindings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoletoAuthories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoleID = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    AuthoryID = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoletoAuthories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stockmovements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<string>(type: "text", nullable: true),
                    Activestockid = table.Column<string>(type: "text", nullable: true),
                    Movementtype = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<double>(type: "double", nullable: false),
                    Prevvalue = table.Column<double>(type: "double", nullable: false),
                    Newvalue = table.Column<double>(type: "double", nullable: false),
                    Movementdate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stockmovements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Unitid = table.Column<string>(type: "text", nullable: true),
                    Stationtid = table.Column<string>(type: "text", nullable: true),
                    Departmentid = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Todogroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todogroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Todos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Todoname = table.Column<string>(type: "text", nullable: true),
                    IsRequired = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    Unittype = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnittoDepartments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UnitId = table.Column<string>(type: "text", nullable: true),
                    DepartmentId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnittoDepartments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    NormalizedUsername = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    Email = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Surname = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Town = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    City = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Language = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Defaultdepartment = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsertoDepartment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<string>(type: "text", nullable: true),
                    DepartmanID = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsertoDepartment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsertoRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    RoleID = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsertoRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsertoSalt",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    Salt = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsertoSalt", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsertoStations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<string>(type: "text", nullable: true),
                    StationID = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsertoStations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activepatientmovementstotodos");

            migrationBuilder.DropTable(
                name: "Activepatients");

            migrationBuilder.DropTable(
                name: "ActivepatientstoFiles");

            migrationBuilder.DropTable(
                name: "ActivepatienttoActivestocks");

            migrationBuilder.DropTable(
                name: "Activestock");

            migrationBuilder.DropTable(
                name: "Authories");

            migrationBuilder.DropTable(
                name: "Cases");

            migrationBuilder.DropTable(
                name: "CasetoDepartments");

            migrationBuilder.DropTable(
                name: "Costumertypes");

            migrationBuilder.DropTable(
                name: "CostumertypetoDepartments");

            migrationBuilder.DropTable(
                name: "Datatables");

            migrationBuilder.DropTable(
                name: "Deactivestocks");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "DepartmenttoStation");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Patientactivestocks");

            migrationBuilder.DropTable(
                name: "Patientapplicants");

            migrationBuilder.DropTable(
                name: "Patientbodycontrolforms");

            migrationBuilder.DropTable(
                name: "Patientdiagnosis");

            migrationBuilder.DropTable(
                name: "Patientdisabilitypermitforms");

            migrationBuilder.DropTable(
                name: "Patientdisabledhealthboardreports");

            migrationBuilder.DropTable(
                name: "Patientfirstadmissionforms");

            migrationBuilder.DropTable(
                name: "Patientfirstapproachreports");

            migrationBuilder.DropTable(
                name: "Patientmovements");

            migrationBuilder.DropTable(
                name: "Patientownershiprecieves");

            migrationBuilder.DropTable(
                name: "Patientrecieveforms");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Patientsubmittingforms");

            migrationBuilder.DropTable(
                name: "Patienttypes");

            migrationBuilder.DropTable(
                name: "Remindings");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "RoletoAuthories");

            migrationBuilder.DropTable(
                name: "Stations");

            migrationBuilder.DropTable(
                name: "Stockmovements");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropTable(
                name: "Todogroups");

            migrationBuilder.DropTable(
                name: "Todos");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "UnittoDepartments");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UsertoDepartment");

            migrationBuilder.DropTable(
                name: "UsertoRoles");

            migrationBuilder.DropTable(
                name: "UsertoSalt");

            migrationBuilder.DropTable(
                name: "UsertoStations");
        }
    }
}
