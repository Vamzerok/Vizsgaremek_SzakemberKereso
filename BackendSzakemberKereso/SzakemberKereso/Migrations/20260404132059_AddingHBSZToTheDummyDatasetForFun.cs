using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SzakemberKereso.Migrations
{
    /// <inheritdoc />
    public partial class AddingHBSZToTheDummyDatasetForFun : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "settlements",
                columns: new[] { "settlement_id", "county_name", "name", "postal_code" },
                values: new object[] { 9, "Vas", "Szombathely", 9700 });

            migrationBuilder.InsertData(
                table: "residential_addresses",
                columns: new[] { "residential_address_id", "building_name", "building_number", "door_number", "floor_number", "settlement_id", "street_name" },
                values: new object[] { 7, null, 12, null, null, 9, "Zrínyi Ilona u." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "settlements",
                keyColumn: "settlement_id",
                keyValue: 9);
        }
    }
}
