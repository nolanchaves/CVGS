using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVGS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_GameId",
                table: "OrderDetails",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Games_GameId",
                table: "OrderDetails",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "GameID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Games_GameId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_GameId",
                table: "OrderDetails");
        }
    }
}
