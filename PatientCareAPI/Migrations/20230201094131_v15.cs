using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace PatientCareAPI.Migrations
{
    public partial class v15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Periods_Checkperiods_CheckperiodModelId",
                table: "Periods");

            migrationBuilder.DropIndex(
                name: "IX_Periods_CheckperiodModelId",
                table: "Periods");

            migrationBuilder.DropColumn(
                name: "CheckperiodModelId",
                table: "Periods");

            migrationBuilder.CreateTable(
                name: "Mailsettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    User = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Smtphost = table.Column<string>(type: "text", nullable: true),
                    Smtpport = table.Column<string>(type: "text", nullable: true),
                    Mailaddress = table.Column<string>(type: "text", nullable: true),
                    Isbodyhtml = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Issettingactive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    CreatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    UpdatedUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    DeleteUser = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeleteTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mailsettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resetpasswordrequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "text", nullable: true),
                    Hashkey = table.Column<string>(type: "text", nullable: true),
                    Expiretime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resetpasswordrequests", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mailsettings");

            migrationBuilder.DropTable(
                name: "Resetpasswordrequests");

            migrationBuilder.AddColumn<int>(
                name: "CheckperiodModelId",
                table: "Periods",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Periods_CheckperiodModelId",
                table: "Periods",
                column: "CheckperiodModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Periods_Checkperiods_CheckperiodModelId",
                table: "Periods",
                column: "CheckperiodModelId",
                principalTable: "Checkperiods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
