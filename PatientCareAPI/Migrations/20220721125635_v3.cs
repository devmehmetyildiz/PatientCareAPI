using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace PatientCareAPI.Migrations
{
    public partial class v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "Patients",
                newName: "Town");

            migrationBuilder.AlterColumn<string>(
                name: "CountryID",
                table: "Patients",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(11)",
                oldMaxLength: 11,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address1",
                table: "Patients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "Patients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Patients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Costumertypeid",
                table: "Patients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Patients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Dateofbirth",
                table: "Patients",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Dateofdeath",
                table: "Patients",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lastname",
                table: "Patients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Floornumber",
                table: "Activepatients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Roomnumber",
                table: "Activepatients",
                type: "text",
                nullable: true);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Costumertypes");

            migrationBuilder.DropTable(
                name: "CostumertypetoDepartments");

            migrationBuilder.DropColumn(
                name: "Address1",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Costumertypeid",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Dateofbirth",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Dateofdeath",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Lastname",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Floornumber",
                table: "Activepatients");

            migrationBuilder.DropColumn(
                name: "Roomnumber",
                table: "Activepatients");

            migrationBuilder.RenameColumn(
                name: "Town",
                table: "Patients",
                newName: "Surname");

            migrationBuilder.AlterColumn<string>(
                name: "CountryID",
                table: "Patients",
                type: "varchar(11)",
                maxLength: 11,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
