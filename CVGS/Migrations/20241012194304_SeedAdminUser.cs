using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVGS.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a3aaa957-39cd-416a-b6ef-829570100f5d", null, "Admin", null });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AddressId", "BirthDate", "ConcurrencyStamp", "DisplayName", "Email", "EmailConfirmed", "FullName", "Gender", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PreferenceId", "ReceivePromotionalEmails", "Role", "SecurityStamp", "ShippingAddressId", "TwoFactorEnabled", "UserName" },
                values: new object[] { "20d6ac16-321a-4471-96bd-940575d0ecc5", 0, null, null, "272af692-23fd-4735-83f8-638414c10081", "Administrator", "admin@example.com", true, null, null, false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEGu4tmT6O/1bXveC6DiUp4QVXZ01CF4ymMTzowflTc8k4mv2vEfZLpji+VQLBLzvSA==", null, false, null, null, "Admin", "c2184e64-9e72-4198-83d7-c6f7b69f3d66", null, false, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a3aaa957-39cd-416a-b6ef-829570100f5d");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "20d6ac16-321a-4471-96bd-940575d0ecc5");
        }
    }
}
