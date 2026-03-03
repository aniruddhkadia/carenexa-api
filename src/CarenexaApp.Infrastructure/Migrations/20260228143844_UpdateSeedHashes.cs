using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarenexaApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedHashes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Users SET PasswordHash = '$2a$12$2eF/QqbC7PlfPmdDTnRTLuqiCGwCb.cB2NosYgBXsJ6YbuBGSUaji' WHERE Email IN ('admin@carenexa.com', 'doctor@carenexa.com')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
