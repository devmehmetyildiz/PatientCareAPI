using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PatientCareAPI.Migrations
{
    public partial class V13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "Deactivestocks",
                type: "double",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Deactivetime",
                table: "Activestock",
                type: "datetime",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deactivetime",
                table: "Activestock");

            migrationBuilder.AlterColumn<string>(
                name: "Amount",
                table: "Deactivestocks",
                type: "text",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double");
        }
    }
}
