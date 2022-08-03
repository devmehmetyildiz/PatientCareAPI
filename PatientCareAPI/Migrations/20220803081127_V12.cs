using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace PatientCareAPI.Migrations
{
    public partial class V12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Deactivestocks",
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
                    table.PrimaryKey("PK_Deactivestocks", x => x.Id);
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
                name: "Patientmovements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Patientid = table.Column<string>(type: "text", nullable: true),
                    Movementtype = table.Column<int>(type: "int", nullable: false),
                    Movementdate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patientmovements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stockmovements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Activestockid = table.Column<string>(type: "text", nullable: true),
                    UserID = table.Column<string>(type: "text", nullable: true),
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deactivestocks");

            migrationBuilder.DropTable(
                name: "Patientactivestocks");

            migrationBuilder.DropTable(
                name: "Patientmovements");

            migrationBuilder.DropTable(
                name: "Stockmovements");
        }
    }
}
