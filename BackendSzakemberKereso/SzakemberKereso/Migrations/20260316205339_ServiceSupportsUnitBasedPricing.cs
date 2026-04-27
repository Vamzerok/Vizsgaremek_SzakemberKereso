using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SzakemberKereso.Migrations
{
    /// <inheritdoc />
    public partial class ServiceSupportsUnitBasedPricing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "max_price",
                table: "services");

            migrationBuilder.RenameColumn(
                name: "min_price",
                table: "services",
                newName: "fixed_price");

            migrationBuilder.AlterColumn<int>(
                name: "duration_in_minutes",
                table: "services",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "pricing_type",
                table: "services",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "unit_name",
                table: "services",
                type: "varchar(64)",
                maxLength: 64,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "unit_price",
                table: "services",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "service_id",
                keyValue: 1,
                columns: new[] { "duration_in_minutes", "fixed_price", "pricing_type", "unit_name", "unit_price" },
                values: new object[] { 15, 2000m, 1, "sarokszelep", 1000m });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "service_id",
                keyValue: 2,
                columns: new[] { "duration_in_minutes", "fixed_price", "pricing_type", "unit_name", "unit_price" },
                values: new object[] { 20, 3000m, 1, "lámpa", 1500m });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "service_id",
                keyValue: 3,
                columns: new[] { "duration_in_minutes", "fixed_price", "pricing_type", "unit_name", "unit_price" },
                values: new object[] { 45, 20000m, 0, null, null });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "service_id",
                keyValue: 4,
                columns: new[] { "duration_in_minutes", "fixed_price", "pricing_type", "unit_name", "unit_price" },
                values: new object[] { null, 10000m, 1, "m2", 1000m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pricing_type",
                table: "services");

            migrationBuilder.DropColumn(
                name: "unit_name",
                table: "services");

            migrationBuilder.DropColumn(
                name: "unit_price",
                table: "services");

            migrationBuilder.RenameColumn(
                name: "fixed_price",
                table: "services",
                newName: "min_price");

            migrationBuilder.AlterColumn<int>(
                name: "duration_in_minutes",
                table: "services",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "max_price",
                table: "services",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "service_id",
                keyValue: 1,
                columns: new[] { "duration_in_minutes", "max_price", "min_price" },
                values: new object[] { 0, 0m, 0m });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "service_id",
                keyValue: 2,
                columns: new[] { "duration_in_minutes", "max_price", "min_price" },
                values: new object[] { 0, 0m, 0m });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "service_id",
                keyValue: 3,
                columns: new[] { "duration_in_minutes", "max_price", "min_price" },
                values: new object[] { 0, 0m, 0m });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "service_id",
                keyValue: 4,
                columns: new[] { "duration_in_minutes", "max_price", "min_price" },
                values: new object[] { 0, 0m, 0m });
        }
    }
}
