using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVGS.Migrations
{
    /// <inheritdoc />
    public partial class dbPreferenceAndAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_AspNetUsers_UserId",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Address_ShippingAddressAddressId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Preference_AspNetUsers_UserId",
                table: "Preference");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Preference",
                table: "Preference");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Address",
                table: "Address");

            migrationBuilder.RenameTable(
                name: "Preference",
                newName: "Preferences");

            migrationBuilder.RenameTable(
                name: "Address",
                newName: "Addresses");

            migrationBuilder.RenameIndex(
                name: "IX_Preference_UserId",
                table: "Preferences",
                newName: "IX_Preferences_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Address_UserId",
                table: "Addresses",
                newName: "IX_Addresses_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Preferences",
                table: "Preferences",
                column: "PreferenceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AspNetUsers_UserId",
                table: "Addresses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Addresses_ShippingAddressAddressId",
                table: "AspNetUsers",
                column: "ShippingAddressAddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Preferences_AspNetUsers_UserId",
                table: "Preferences",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AspNetUsers_UserId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Addresses_ShippingAddressAddressId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Preferences_AspNetUsers_UserId",
                table: "Preferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Preferences",
                table: "Preferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses");

            migrationBuilder.RenameTable(
                name: "Preferences",
                newName: "Preference");

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "Address");

            migrationBuilder.RenameIndex(
                name: "IX_Preferences_UserId",
                table: "Preference",
                newName: "IX_Preference_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_UserId",
                table: "Address",
                newName: "IX_Address_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Preference",
                table: "Preference",
                column: "PreferenceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Address",
                table: "Address",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_AspNetUsers_UserId",
                table: "Address",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Address_ShippingAddressAddressId",
                table: "AspNetUsers",
                column: "ShippingAddressAddressId",
                principalTable: "Address",
                principalColumn: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Preference_AspNetUsers_UserId",
                table: "Preference",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
