using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVGS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGameSeeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Available",
                table: "Games");

            migrationBuilder.AlterColumn<double>(
                name: "Rating",
                table: "Games",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 1,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 2,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 3,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 4,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 5,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 6,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 7,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 8,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 9,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 10,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 11,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 12,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 13,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 14,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 15,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 16,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 17,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 18,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 19,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 20,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 21,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 22,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 23,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 24,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 25,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 26,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 27,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 28,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 29,
                column: "Rating",
                value: null);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 30,
                column: "Rating",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Rating",
                table: "Games",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Available",
                table: "Games",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 1,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 9.1000003814697266 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 2,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 9.3999996185302734 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 3,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 8.5 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 4,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 9.0 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 5,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 9.3000001907348633 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 6,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 9.0 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 7,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 9.3000001907348633 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 8,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 8.6999998092651367 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 9,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 8.0 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 10,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 8.8999996185302734 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 11,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 8.6999998092651367 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 12,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 9.6000003814697266 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 13,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 9.1000003814697266 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 14,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 8.8000001907348633 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 15,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 9.1000003814697266 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 16,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 9.3000001907348633 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 17,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 9.5 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 18,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 9.1000003814697266 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 19,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 9.0 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 20,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 8.8000001907348633 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 21,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 9.0 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 22,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 9.5 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 23,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 9.1999998092651367 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 24,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 9.1000003814697266 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 25,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 8.5 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 26,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 9.6999998092651367 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 27,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 9.3000001907348633 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 28,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 9.5 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 29,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 9.1999998092651367 });

            migrationBuilder.UpdateData(
                table: "Games",
                keyColumn: "GameID",
                keyValue: 30,
                columns: new[] { "Available", "Rating" },
                values: new object[] { true, 8.8000001907348633 });
        }
    }
}
