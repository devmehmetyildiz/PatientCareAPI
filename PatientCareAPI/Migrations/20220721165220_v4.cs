using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace PatientCareAPI.Migrations
{
    public partial class v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "Stocks",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Activepatients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    PatientID = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    Registerdate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Releasedate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Processid = table.Column<string>(type: "text", nullable: true),
                    Floornumber = table.Column<string>(type: "text", nullable: true),
                    Roomnumber = table.Column<string>(type: "text", nullable: true),
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
                name: "Activestock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    Stockid = table.Column<string>(type: "text", nullable: true),
                    Skt = table.Column<string>(type: "text", nullable: true),
                    Barcodeno = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<double>(type: "double", nullable: false),
                    Purchaseprice = table.Column<double>(type: "double", nullable: false),
                    Purchasedate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Info = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activestock", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Lastname = table.Column<string>(type: "text", nullable: true),
                    CountryID = table.Column<string>(type: "text", nullable: true),
                    Patienttypeid = table.Column<string>(type: "text", nullable: true),
                    Costumertypeid = table.Column<string>(type: "text", nullable: true),
                    Address1 = table.Column<string>(type: "text", nullable: true),
                    Address2 = table.Column<string>(type: "text", nullable: true),
                    Town = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    Dateofbirth = table.Column<DateTime>(type: "datetime", nullable: true),
                    Dateofdeath = table.Column<DateTime>(type: "datetime", nullable: true),
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activepatients");

            migrationBuilder.DropTable(
                name: "Activestock");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Stocks");
        }
    }
}
