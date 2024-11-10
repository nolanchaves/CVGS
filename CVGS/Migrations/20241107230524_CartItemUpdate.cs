using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVGS.Migrations
{
    /// <inheritdoc />
    public partial class CartItemUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImageURL",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "CartItems");

            migrationBuilder.AddColumn<int>(
                name: "GameID",
                table: "CartItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_GameID",
                table: "CartItems",
                column: "GameID");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Games_GameID",
                table: "CartItems",
                column: "GameID",
                principalTable: "Games",
                principalColumn: "GameID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Games_GameID",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_GameID",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "GameID",
                table: "CartItems");

            migrationBuilder.AddColumn<string>(
                name: "CoverImageURL",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "CartItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
