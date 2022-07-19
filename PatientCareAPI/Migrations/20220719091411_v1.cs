using Microsoft.EntityFrameworkCore.Migrations;

namespace PatientCareAPI.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Stationtid",
                table: "Stocks",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stationtid",
                table: "Stocks");
        }
    }
}
