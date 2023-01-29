using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace PatientCareAPI.Migrations
{
    public partial class v7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Todos",
                table: "Todos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Todogroups",
                table: "Todogroups");

            migrationBuilder.RenameTable(
                name: "Todos",
                newName: "TododefineModel");

            migrationBuilder.RenameTable(
                name: "Todogroups",
                newName: "TodogroupdefineModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TododefineModel",
                table: "TododefineModel",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TodogroupdefineModel",
                table: "TodogroupdefineModel",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TodogrouptoTodos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    GroupID = table.Column<string>(type: "text", nullable: true),
                    TodoID = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodogrouptoTodos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodogrouptoTodos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TodogroupdefineModel",
                table: "TodogroupdefineModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TododefineModel",
                table: "TododefineModel");

            migrationBuilder.RenameTable(
                name: "TodogroupdefineModel",
                newName: "Todogroups");

            migrationBuilder.RenameTable(
                name: "TododefineModel",
                newName: "Todos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Todogroups",
                table: "Todogroups",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Todos",
                table: "Todos",
                column: "Id");
        }
    }
}
