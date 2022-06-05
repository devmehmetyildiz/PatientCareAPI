using Microsoft.EntityFrameworkCore.Migrations;

namespace PatientCareAPI.Migrations
{
    public partial class Web1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsertoDepartment",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "text", nullable: true),
                    DepartmanID = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "UsertoStations",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "text", nullable: true),
                    StationID = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsertoDepartment");

            migrationBuilder.DropTable(
                name: "UsertoStations");
        }
    }
}
