using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVGS.Migrations
{
    /// <inheritdoc />
    public partial class AddOrdersAndOrderDetailsGameType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GameType",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameType",
                table: "OrderDetails");
        }
    }
}
