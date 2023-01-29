using Microsoft.EntityFrameworkCore.Migrations;

namespace PatientCareAPI.Migrations
{
    public partial class v9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TodogroupdefineModel",
                table: "TodogroupdefineModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TododefineModel",
                table: "TododefineModel");

            migrationBuilder.RenameTable(
                name: "TodogroupdefineModel",
                newName: "Todogroupdefines");

            migrationBuilder.RenameTable(
                name: "TododefineModel",
                newName: "Tododefines");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Todogroupdefines",
                table: "Todogroupdefines",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tododefines",
                table: "Tododefines",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Todogroupdefines",
                table: "Todogroupdefines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tododefines",
                table: "Tododefines");

            migrationBuilder.RenameTable(
                name: "Todogroupdefines",
                newName: "TodogroupdefineModel");

            migrationBuilder.RenameTable(
                name: "Tododefines",
                newName: "TododefineModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TodogroupdefineModel",
                table: "TodogroupdefineModel",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TododefineModel",
                table: "TododefineModel",
                column: "Id");
        }
    }
}
