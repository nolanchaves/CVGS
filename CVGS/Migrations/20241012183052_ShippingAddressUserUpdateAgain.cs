using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVGS.Migrations
{
    /// <inheritdoc />
    public partial class ShippingAddressUserUpdateAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StreetAddress",
                table: "ShippingAddresses",
                newName: "ShippingStreetAddress");

            migrationBuilder.RenameColumn(
                name: "Province",
                table: "ShippingAddresses",
                newName: "ShippingProvince");

            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "ShippingAddresses",
                newName: "ShippingPostalCode");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "ShippingAddresses",
                newName: "ShippingCountry");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "ShippingAddresses",
                newName: "ShippingCity");

            migrationBuilder.RenameColumn(
                name: "AptSuite",
                table: "ShippingAddresses",
                newName: "ShippingAptSuite");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShippingStreetAddress",
                table: "ShippingAddresses",
                newName: "StreetAddress");

            migrationBuilder.RenameColumn(
                name: "ShippingProvince",
                table: "ShippingAddresses",
                newName: "Province");

            migrationBuilder.RenameColumn(
                name: "ShippingPostalCode",
                table: "ShippingAddresses",
                newName: "PostalCode");

            migrationBuilder.RenameColumn(
                name: "ShippingCountry",
                table: "ShippingAddresses",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "ShippingCity",
                table: "ShippingAddresses",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "ShippingAptSuite",
                table: "ShippingAddresses",
                newName: "AptSuite");
        }
    }
}
