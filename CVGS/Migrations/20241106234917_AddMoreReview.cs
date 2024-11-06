using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CVGS.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Review",
                columns: new[] { "Id", "Approved", "Content", "GameId", "Rating", "UserId" },
                values: new object[,]
                {
                    { 2, false, "sui", 11, 5, "042f95a1-3247-4e6a-a375-d2165a8bf16c" },
                    { 3, true, "john uncharted", 21, 3, "88193658-5295-478b-9f7e-534f739a06bc" },
                    { 4, true, "very open", 26, null, "bac4f198-0003-438a-a347-2c27bc0e0ffa" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
