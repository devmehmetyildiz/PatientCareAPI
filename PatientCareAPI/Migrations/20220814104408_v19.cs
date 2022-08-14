using Microsoft.EntityFrameworkCore.Migrations;

namespace PatientCareAPI.Migrations
{
    public partial class v19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "Departmentid",
            //    table: "Activepatients");

            //migrationBuilder.DropColumn(
            //    name: "Departmentname",
            //    table: "Activepatients");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Departmentid",
                table: "Activepatients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Departmentname",
                table: "Activepatients",
                type: "text",
                nullable: true);
        }
    }
}
