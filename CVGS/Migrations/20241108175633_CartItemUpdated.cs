using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVGS.Migrations
{
    /// <inheritdoc />
    public partial class CartItemUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Games_GameID",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "GameID",
                table: "CartItems",
                newName: "GameId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CartItems",
                newName: "CartItemId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_GameID",
                table: "CartItems",
                newName: "IX_CartItems_GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Games_GameId",
                table: "CartItems",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Games_GameId",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "CartItems",
                newName: "GameID");

            migrationBuilder.RenameColumn(
                name: "CartItemId",
                table: "CartItems",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_GameId",
                table: "CartItems",
                newName: "IX_CartItems_GameID");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "CartItems",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Games_GameID",
                table: "CartItems",
                column: "GameID",
                principalTable: "Games",
                principalColumn: "GameID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
