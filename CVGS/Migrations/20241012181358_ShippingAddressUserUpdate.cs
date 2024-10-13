using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVGS.Migrations
{
    /// <inheritdoc />
    public partial class ShippingAddressUserUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Addresses_ShippingAddressAddressId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ShippingAddressAddressId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ShippingAddressAddressId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "ShippingAddressId",
                table: "Addresses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShippingAddresses",
                columns: table => new
                {
                    ShippingAddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StreetAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AptSuite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Province = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingAddresses", x => x.ShippingAddressId);
                    table.ForeignKey(
                        name: "FK_ShippingAddresses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_ShippingAddressId",
                table: "Addresses",
                column: "ShippingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingAddresses_UserId",
                table: "ShippingAddresses",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_ShippingAddresses_ShippingAddressId",
                table: "Addresses",
                column: "ShippingAddressId",
                principalTable: "ShippingAddresses",
                principalColumn: "ShippingAddressId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_ShippingAddresses_ShippingAddressId",
                table: "Addresses");

            migrationBuilder.DropTable(
                name: "ShippingAddresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_ShippingAddressId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "ShippingAddressId",
                table: "Addresses");

            migrationBuilder.AddColumn<int>(
                name: "ShippingAddressAddressId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ShippingAddressAddressId",
                table: "AspNetUsers",
                column: "ShippingAddressAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Addresses_ShippingAddressAddressId",
                table: "AspNetUsers",
                column: "ShippingAddressAddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId");
        }
    }
}
