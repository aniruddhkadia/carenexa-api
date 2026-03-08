using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AroviaApp.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Phase5SchemaAndSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClinicId",
                table: "VisitTemplates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ClinicId",
                table: "MedicalRecords",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_VisitTemplates_ClinicId",
                table: "VisitTemplates",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecords_ClinicId",
                table: "MedicalRecords",
                column: "ClinicId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_Clinics_ClinicId",
                table: "MedicalRecords",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitTemplates_Clinics_ClinicId",
                table: "VisitTemplates",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_Clinics_ClinicId",
                table: "MedicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitTemplates_Clinics_ClinicId",
                table: "VisitTemplates");

            migrationBuilder.DropIndex(
                name: "IX_VisitTemplates_ClinicId",
                table: "VisitTemplates");

            migrationBuilder.DropIndex(
                name: "IX_MedicalRecords_ClinicId",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "ClinicId",
                table: "VisitTemplates");

            migrationBuilder.DropColumn(
                name: "ClinicId",
                table: "MedicalRecords");
        }
    }
}
