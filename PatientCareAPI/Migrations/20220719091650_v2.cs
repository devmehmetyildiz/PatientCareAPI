using Microsoft.EntityFrameworkCore.Migrations;

namespace PatientCareAPI.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Barcodeno",
                table: "Stocks");

            migrationBuilder.AddColumn<string>(
                name: "Barcodeno",
                table: "Activestock",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Barcodeno",
                table: "Activestock");

            migrationBuilder.AddColumn<string>(
                name: "Barcodeno",
                table: "Stocks",
                type: "text",
                nullable: true);
        }
    }
}
