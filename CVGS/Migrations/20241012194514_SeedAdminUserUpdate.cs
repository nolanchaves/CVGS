using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVGS.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUserUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a3aaa957-39cd-416a-b6ef-829570100f5d");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "20d6ac16-321a-4471-96bd-940575d0ecc5");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f9504e3f-c9c0-4517-a4ab-018dd6210a15", null, "Admin", null });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AddressId", "BirthDate", "ConcurrencyStamp", "DisplayName", "Email", "EmailConfirmed", "FullName", "Gender", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PreferenceId", "ReceivePromotionalEmails", "Role", "SecurityStamp", "ShippingAddressId", "TwoFactorEnabled", "UserName" },
                values: new object[] { "0101c3c8-4a3b-4d75-8f3c-3d04688d6627", 0, null, null, "0fb0ba80-22c8-459a-8150-ed5452d7d89d", "Administrator", "admin@example.com", true, null, null, false, null, "ADMIN@EXAMPLE.COM", "ADMIN", null, null, false, null, null, "Admin", "a4834354-5f25-4086-91ad-9f4cc01d0c9b", null, false, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f9504e3f-c9c0-4517-a4ab-018dd6210a15");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0101c3c8-4a3b-4d75-8f3c-3d04688d6627");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a3aaa957-39cd-416a-b6ef-829570100f5d", null, "Admin", null });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AddressId", "BirthDate", "ConcurrencyStamp", "DisplayName", "Email", "EmailConfirmed", "FullName", "Gender", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PreferenceId", "ReceivePromotionalEmails", "Role", "SecurityStamp", "ShippingAddressId", "TwoFactorEnabled", "UserName" },
                values: new object[] { "20d6ac16-321a-4471-96bd-940575d0ecc5", 0, null, null, "272af692-23fd-4735-83f8-638414c10081", "Administrator", "admin@example.com", true, null, null, false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEGu4tmT6O/1bXveC6DiUp4QVXZ01CF4ymMTzowflTc8k4mv2vEfZLpji+VQLBLzvSA==", null, false, null, null, "Admin", "c2184e64-9e72-4198-83d7-c6f7b69f3d66", null, false, "admin" });
        }
    }
}
