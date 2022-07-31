using Microsoft.EntityFrameworkCore.Migrations;

namespace PatientCareAPI.Migrations
{
    public partial class v9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Stocks");

            migrationBuilder.AddColumn<string>(
                name: "Defaultdepartment",
                table: "Users",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Defaultdepartment",
                table: "Users");

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "Stocks",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
