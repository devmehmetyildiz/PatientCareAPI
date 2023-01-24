using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PatientCareAPI.Migrations
{
    public partial class v5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activepatientid",
                table: "Patientmovements");

            migrationBuilder.DropColumn(
                name: "Movementid",
                table: "Patientmovements");

            migrationBuilder.DropColumn(
                name: "NewStatus",
                table: "Patientmovements");

            migrationBuilder.DropColumn(
                name: "Oldstatus",
                table: "Patientmovements");

            //migrationBuilder.RenameColumn(
            //    name: "Todoname",
            //    table: "Todos",
            //    newName: "TodogroupID");

            migrationBuilder.Sql("ALTER TABLE Todos RENAME COLUMN Todoname TO TodogroupID");

            //migrationBuilder.RenameColumn(
            //    name: "UserID",
            //    table: "Patientmovements",
            //    newName: "PatientID");

            migrationBuilder.Sql("ALTER TABLE Patientmovements RENAME COLUMN UserID TO PatientID");

            //migrationBuilder.RenameColumn(
            //    name: "Movementtype",
            //    table: "Patientmovements",
            //    newName: "Patientmovementtype");

            migrationBuilder.Sql("ALTER TABLE Patientmovements RENAME COLUMN Movementtype TO Patientmovementtype");

            //migrationBuilder.RenameColumn(
            //    name: "IsTodoneeded",
            //    table: "Patientmovements",
            //    newName: "IsTodoneed");

            migrationBuilder.Sql("ALTER TABLE Patientmovements RENAME COLUMN IsTodoneeded TO IsTodoneed");

            migrationBuilder.AddColumn<string>(
                name: "Info",
                table: "Todos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNeedactivation",
                table: "Todos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Todos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentID",
                table: "Todogroups",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Stocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Stockmovements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Purchaseorderstocksmovements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Purchaseorderstocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Patientstocksmovements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Patientstocks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "WarehouseID",
                table: "Patients",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsComplated",
                table: "Patientmovements",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Patientmovements",
                type: "varchar(85)",
                maxLength: 85,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "Patientmovements",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUser",
                table: "Patientmovements",
                type: "varchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteTime",
                table: "Patientmovements",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeleteUser",
                table: "Patientmovements",
                type: "varchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Patientmovements",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTodocompleted",
                table: "Patientmovements",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NewPatientmovementtype",
                table: "Patientmovements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OldPatientmovementtype",
                table: "Patientmovements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "Patientmovements",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedUser",
                table: "Patientmovements",
                type: "varchar(30)",
                maxLength: 30,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Info",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "IsNeedactivation",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "DepartmentID",
                table: "Todogroups");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Stockmovements");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Purchaseorderstocksmovements");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Purchaseorderstocks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Patientstocksmovements");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Patientstocks");

            migrationBuilder.DropColumn(
                name: "WarehouseID",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Patientmovements");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "Patientmovements");

            migrationBuilder.DropColumn(
                name: "CreatedUser",
                table: "Patientmovements");

            migrationBuilder.DropColumn(
                name: "DeleteTime",
                table: "Patientmovements");

            migrationBuilder.DropColumn(
                name: "DeleteUser",
                table: "Patientmovements");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Patientmovements");

            migrationBuilder.DropColumn(
                name: "IsTodocompleted",
                table: "Patientmovements");

            migrationBuilder.DropColumn(
                name: "NewPatientmovementtype",
                table: "Patientmovements");

            migrationBuilder.DropColumn(
                name: "OldPatientmovementtype",
                table: "Patientmovements");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "Patientmovements");

            migrationBuilder.DropColumn(
                name: "UpdatedUser",
                table: "Patientmovements");

            migrationBuilder.RenameColumn(
                name: "TodogroupID",
                table: "Todos",
                newName: "Todoname");

            migrationBuilder.RenameColumn(
                name: "Patientmovementtype",
                table: "Patientmovements",
                newName: "Movementtype");

            migrationBuilder.RenameColumn(
                name: "PatientID",
                table: "Patientmovements",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "IsTodoneed",
                table: "Patientmovements",
                newName: "IsTodoneeded");

            migrationBuilder.AlterColumn<string>(
                name: "IsComplated",
                table: "Patientmovements",
                type: "text",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AddColumn<string>(
                name: "Activepatientid",
                table: "Patientmovements",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Movementid",
                table: "Patientmovements",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewStatus",
                table: "Patientmovements",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Oldstatus",
                table: "Patientmovements",
                type: "text",
                nullable: true);
        }
    }
}
