using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace PatientCareAPI.Migrations
{
    public partial class v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DeactivestockModel",
                table: "DeactivestockModel");

            migrationBuilder.RenameTable(
                name: "DeactivestockModel",
                newName: "Deactivestocks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Deactivestocks",
                table: "Deactivestocks",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
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
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Deactivestocks",
                table: "Deactivestocks");

            migrationBuilder.RenameTable(
                name: "Deactivestocks",
                newName: "DeactivestockModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeactivestockModel",
                table: "DeactivestockModel",
                column: "Id");
        }
    }
}
