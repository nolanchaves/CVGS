using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVGS.Migrations
{
    /// <inheritdoc />
    public partial class dbPreferenceUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvailablePlatforms",
                table: "Preferences",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailablePlatforms",
                table: "Preferences");
        }
    }
}
