using Microsoft.EntityFrameworkCore.Migrations;

namespace PatientCareAPI.Migrations
{
    public partial class v18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Activepatientid",
                table: "Patientsubmittingforms",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Activepatientid",
                table: "Patientrecieveforms",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Activepatientid",
                table: "Patientownershiprecieves",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActivepatientID",
                table: "Patientfirstapproachreports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActivepatientID",
                table: "Patientfirstadmissionforms",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Activepatientid",
                table: "Patientdisabilitypermitforms",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activepatientid",
                table: "Patientsubmittingforms");

            migrationBuilder.DropColumn(
                name: "Activepatientid",
                table: "Patientrecieveforms");

            migrationBuilder.DropColumn(
                name: "Activepatientid",
                table: "Patientownershiprecieves");

            migrationBuilder.DropColumn(
                name: "ActivepatientID",
                table: "Patientfirstapproachreports");

            migrationBuilder.DropColumn(
                name: "ActivepatientID",
                table: "Patientfirstadmissionforms");

            migrationBuilder.DropColumn(
                name: "Activepatientid",
                table: "Patientdisabilitypermitforms");
        }
    }
}
