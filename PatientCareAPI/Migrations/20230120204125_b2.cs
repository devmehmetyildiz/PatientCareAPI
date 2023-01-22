using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace PatientCareAPI.Migrations
{
    public partial class b2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Datatables");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Datatables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Json = table.Column<string>(type: "text", nullable: true),
                    Tablename = table.Column<string>(type: "text", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Datatables", x => x.Id);
                });
        }
    }
}
