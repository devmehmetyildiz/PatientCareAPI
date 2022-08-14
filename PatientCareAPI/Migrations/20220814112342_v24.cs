using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace PatientCareAPI.Migrations
{
    public partial class v24 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activepatients");
        }
    }
}
