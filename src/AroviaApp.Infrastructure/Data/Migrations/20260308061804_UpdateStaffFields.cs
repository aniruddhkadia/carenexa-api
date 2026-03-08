using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AroviaApp.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStaffFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RenewalDueDate",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RenewalDueDate",
                table: "Users");
        }
    }
}
