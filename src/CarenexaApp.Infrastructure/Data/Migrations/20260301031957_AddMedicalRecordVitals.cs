using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarenexaApp.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMedicalRecordVitals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BP",
                table: "MedicalRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Pulse",
                table: "MedicalRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Temp",
                table: "MedicalRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Weight",
                table: "MedicalRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BP",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "Pulse",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "Temp",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "MedicalRecords");
        }
    }
}
