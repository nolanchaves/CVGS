using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVGS.Migrations
{
    /// <inheritdoc />
    public partial class ShippingAddressEntityUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SameAsShippingAddress",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "SameAsShippingAddress",
                table: "Addresses",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SameAsShippingAddress",
                table: "Addresses");

            migrationBuilder.AddColumn<bool>(
                name: "SameAsShippingAddress",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }
    }
}
