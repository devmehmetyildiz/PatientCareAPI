using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace PatientCareAPI.Migrations
{
    public partial class v17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Patientfirstadmissionforms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
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
                    Patientid = table.Column<string>(type: "text", nullable: true),
                    Movementtype = table.Column<int>(type: "int", nullable: false),
                    Iswaitingactivation = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeactive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Movementdate = table.Column<DateTime>(type: "datetime", nullable: true)
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
                name: "Process",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
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
                    table.PrimaryKey("PK_Process", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcesstoActivestocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ProcessID = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    ActivestocksID = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcesstoActivestocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcesstoFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ProcessID = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    FilesID = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcesstoFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcesstoUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ProcessID = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    UserID = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcesstoUsers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "Process");

            migrationBuilder.DropTable(
                name: "ProcesstoActivestocks");

            migrationBuilder.DropTable(
                name: "ProcesstoFiles");

            migrationBuilder.DropTable(
                name: "ProcesstoUsers");
        }
    }
}
