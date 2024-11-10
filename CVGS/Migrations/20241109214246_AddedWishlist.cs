using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CVGS.Migrations
{
    /// <inheritdoc />
    public partial class AddedWishlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Wishlist",
                columns: table => new
                {
                    WishlistId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishlist", x => x.WishlistId);
                    table.ForeignKey(
                        name: "FK_Wishlist_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wishlist_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Wishlist",
                columns: new[] { "WishlistId", "GameId", "UserId" },
                values: new object[,]
                {
                    { 1, 1, "05e61254-11dc-44d9-89e7-0e574ce7099a" },
                    { 2, 2, "05e61254-11dc-44d9-89e7-0e574ce7099a" },
                    { 3, 11, "05e61254-11dc-44d9-89e7-0e574ce7099a" },
                    { 4, 25, "05e61254-11dc-44d9-89e7-0e574ce7099a" },
                    { 5, 6, "05e61254-11dc-44d9-89e7-0e574ce7099a" },
                    { 6, 3, "6c9c58e6-5b8d-42c5-8cf9-1e7c7480f5d2" },
                    { 7, 4, "6c9c58e6-5b8d-42c5-8cf9-1e7c7480f5d2" },
                    { 8, 28, "6c9c58e6-5b8d-42c5-8cf9-1e7c7480f5d2" },
                    { 9, 13, "6c9c58e6-5b8d-42c5-8cf9-1e7c7480f5d2" },
                    { 10, 30, "6c9c58e6-5b8d-42c5-8cf9-1e7c7480f5d2" },
                    { 11, 30, "042f95a1-3247-4e6a-a375-d2165a8bf16c" },
                    { 12, 23, "042f95a1-3247-4e6a-a375-d2165a8bf16c" },
                    { 13, 20, "042f95a1-3247-4e6a-a375-d2165a8bf16c" },
                    { 14, 1, "042f95a1-3247-4e6a-a375-d2165a8bf16c" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wishlist_GameId",
                table: "Wishlist",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlist_UserId",
                table: "Wishlist",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wishlist");
        }
    }
}
