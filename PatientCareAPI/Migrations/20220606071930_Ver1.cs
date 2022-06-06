using Microsoft.EntityFrameworkCore.Migrations;

namespace PatientCareAPI.Migrations
{
    public partial class Ver1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tbl_Users",
                table: "Tbl_Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tbl_Unit",
                table: "Tbl_Unit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tbl_Stations",
                table: "Tbl_Stations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tbl_Roles",
                table: "Tbl_Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tbl_Department",
                table: "Tbl_Department");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tbl_Case",
                table: "Tbl_Case");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tbl_Authory",
                table: "Tbl_Authory");

            migrationBuilder.RenameTable(
                name: "Tbl_Users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Tbl_Unit",
                newName: "Units");

            migrationBuilder.RenameTable(
                name: "Tbl_Stations",
                newName: "Stations");

            migrationBuilder.RenameTable(
                name: "Tbl_Roles",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "Tbl_Department",
                newName: "Departments");

            migrationBuilder.RenameTable(
                name: "Tbl_Case",
                newName: "Cases");

            migrationBuilder.RenameTable(
                name: "Tbl_Authory",
                newName: "Authories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Units",
                table: "Units",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stations",
                table: "Stations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Departments",
                table: "Departments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cases",
                table: "Cases",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Authories",
                table: "Authories",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DepartmenttoStation",
                columns: table => new
                {
                    DepartmentID = table.Column<string>(type: "text", nullable: true),
                    StationID = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartmenttoStation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Units",
                table: "Units");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stations",
                table: "Stations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Departments",
                table: "Departments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cases",
                table: "Cases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Authories",
                table: "Authories");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Tbl_Users");

            migrationBuilder.RenameTable(
                name: "Units",
                newName: "Tbl_Unit");

            migrationBuilder.RenameTable(
                name: "Stations",
                newName: "Tbl_Stations");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Tbl_Roles");

            migrationBuilder.RenameTable(
                name: "Departments",
                newName: "Tbl_Department");

            migrationBuilder.RenameTable(
                name: "Cases",
                newName: "Tbl_Case");

            migrationBuilder.RenameTable(
                name: "Authories",
                newName: "Tbl_Authory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tbl_Users",
                table: "Tbl_Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tbl_Unit",
                table: "Tbl_Unit",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tbl_Stations",
                table: "Tbl_Stations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tbl_Roles",
                table: "Tbl_Roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tbl_Department",
                table: "Tbl_Department",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tbl_Case",
                table: "Tbl_Case",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tbl_Authory",
                table: "Tbl_Authory",
                column: "Id");
        }
    }
}
