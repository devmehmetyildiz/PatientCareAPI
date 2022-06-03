using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace PatientCareAPI.Migrations
{
    public partial class Verbose : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoletoAuthories",
                columns: table => new
                {
                    RoleID = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    AuthoryID = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Authory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    NormalizedName = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Authory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Case",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CaseGroup = table.Column<string>(type: "text", nullable: true),
                    CaseStatus = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    NormalizedName = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "text", nullable: true),
                    UpdatedUser = table.Column<string>(type: "text", nullable: true),
                    DeleteUser = table.Column<string>(type: "text", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Case", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    NormalizedName = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "text", nullable: true),
                    UpdatedUser = table.Column<string>(type: "text", nullable: true),
                    DeleteUser = table.Column<string>(type: "text", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Unit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UnitGroup = table.Column<string>(type: "text", nullable: true),
                    UnitStatus = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    NormalizedName = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "text", nullable: true),
                    UpdatedUser = table.Column<string>(type: "text", nullable: true),
                    DeleteUser = table.Column<string>(type: "text", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Unit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    NormalizedUsername = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    Email = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(85)", maxLength: 85, nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    Isactive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Surname = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Town = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    City = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Language = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsertoRoles",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    RoleID = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true)
                },
                constraints: table =>
                {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoletoAuthories");

            migrationBuilder.DropTable(
                name: "Tbl_Authory");

            migrationBuilder.DropTable(
                name: "Tbl_Case");

            migrationBuilder.DropTable(
                name: "Tbl_Roles");

            migrationBuilder.DropTable(
                name: "Tbl_Unit");

            migrationBuilder.DropTable(
                name: "Tbl_Users");

            migrationBuilder.DropTable(
                name: "UsertoRoles");

            migrationBuilder.DropTable(
                name: "UsertoSalt");
        }
    }
}
