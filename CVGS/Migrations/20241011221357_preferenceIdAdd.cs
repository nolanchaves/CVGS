using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVGS.Migrations
{
    /// <inheritdoc />
    public partial class preferenceIdAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PreferenceId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreferenceId",
                table: "AspNetUsers");
        }
    }
}
