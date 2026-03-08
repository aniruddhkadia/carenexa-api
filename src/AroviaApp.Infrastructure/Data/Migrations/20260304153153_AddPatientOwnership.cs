using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AroviaApp.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPatientOwnership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "Patients",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("D359BAC2-AD8E-4E85-8CB0-C73697D2EDE6"));

            migrationBuilder.InsertData(
                table: "Medicines",
                columns: new[] { "Id", "BrandName", "CreatedAt", "Form", "GenericName", "IsActive", "Strength" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Dolo", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tablet", "Paracetamol", true, "650mg" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Amoxil", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Capsule", "Amoxicillin", true, "500mg" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Brufen", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tablet", "Ibuprofen", true, "400mg" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Zyrtec", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tablet", "Cetirizine", true, "10mg" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "Pan 40", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tablet", "Pantoprazole", true, "40mg" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), "Azee", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tablet", "Azithromycin", true, "500mg" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patients_CreatedByUserId",
                table: "Patients",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Users_CreatedByUserId",
                table: "Patients",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Users_CreatedByUserId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_CreatedByUserId",
                table: "Patients");

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "Medicines",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"));

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Patients");
        }
    }
}
