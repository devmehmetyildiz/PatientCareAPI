using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace PatientCareAPI.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "UnittoDepartments",
               columns: table => new
               {
                   Id = table.Column<int>(type: "int", nullable: false)
                       .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                   UnitId = table.Column<string>(type: "text", nullable: true),
                   DepartmentId = table.Column<string>(type: "text", nullable: true),
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_UnittoDepartments", x => x.Id);
               });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
