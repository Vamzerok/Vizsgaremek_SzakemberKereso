using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SzakemberKereso.Migrations
{
    /// <inheritdoc />
    public partial class SeedDummyDataExpansion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "occupations",
                columns: new[] { "feor_id", "description", "name" },
                values: new object[,]
                {
                    { 7321, "", "Lakatos" },
                    { 7326, "", "Kovács" },
                    { 7537, "", "Kályha- és kandallóépítő" }
                });

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 1,
                columns: new[] { "building_name", "street_name" },
                values: new object[] { "utca", "Deák Ferenc" });

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 2,
                columns: new[] { "building_name", "street_name" },
                values: new object[] { "utca", "Petőfi Sándor" });

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 3,
                columns: new[] { "building_name", "street_name" },
                values: new object[] { "tér", "Kossuth Lajos" });

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 4,
                columns: new[] { "building_name", "street_name" },
                values: new object[] { "út", "Dózsa György" });

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 5,
                columns: new[] { "building_name", "street_name" },
                values: new object[] { "utca", "Rákóczi Ferenc" });

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 6,
                columns: new[] { "building_name", "street_name" },
                values: new object[] { "tér", "Szabadság" });

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 7,
                columns: new[] { "building_name", "street_name" },
                values: new object[] { "utca", "Zrínyi Ilona" });

            migrationBuilder.InsertData(
                table: "residential_addresses",
                columns: new[] { "residential_address_id", "building_number", "door_number", "floor_number", "building_name", "settlement_id", "street_name" },
                values: new object[,]
                {
                    { 8, 5, null, null, "utca", 9, "Kossuth Lajos" },
                    { 13, 8, null, null, "utca", 11, "Petőfi Sándor" },
                    { 14, 12, null, null, "utca", 9, "Arany János" }
                });

            migrationBuilder.InsertData(
                table: "settlements",
                columns: new[] { "settlement_id", "county_name", "name", "postal_code" },
                values: new object[,]
                {
                    { 12, "Fejér", "Székesfehérvár", 8019 },
                    { 13, "Hajdú-Bihar", "Debrecen", 4042 },
                    { 14, "Veszprém", "Zalahaláp", 8308 },
                    { 15, "Veszprém", "Tapolca", 8300 }
                });

            migrationBuilder.InsertData(
                table: "residential_addresses",
                columns: new[] { "residential_address_id", "building_number", "door_number", "floor_number", "building_name", "settlement_id", "street_name" },
                values: new object[,]
                {
                    { 9, 15, null, null, "utca", 12, "Ady Endre" },
                    { 10, 18, null, null, "tér", 13, "Bem" },
                    { 11, 3, null, null, "utca", 14, "Fő" },
                    { 12, 2, null, null, "utca", 15, "Batsányi János" },
                    { 15, 8, null, null, "körút", 12, "Vár" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "occupations",
                keyColumn: "feor_id",
                keyValue: 7321);

            migrationBuilder.DeleteData(
                table: "occupations",
                keyColumn: "feor_id",
                keyValue: 7326);

            migrationBuilder.DeleteData(
                table: "occupations",
                keyColumn: "feor_id",
                keyValue: 7537);

            migrationBuilder.DeleteData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "settlements",
                keyColumn: "settlement_id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "settlements",
                keyColumn: "settlement_id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "settlements",
                keyColumn: "settlement_id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "settlements",
                keyColumn: "settlement_id",
                keyValue: 15);

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 1,
                columns: new[] { "building_name", "street_name" },
                values: new object[] { null, "Fő utca" });

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 2,
                columns: new[] { "building_name", "street_name" },
                values: new object[] { null, "Petőfi utca" });

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 3,
                columns: new[] { "building_name", "street_name" },
                values: new object[] { null, "Kossuth tér" });

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 4,
                columns: new[] { "building_name", "street_name" },
                values: new object[] { null, "Dózsa György út" });

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 5,
                columns: new[] { "building_name", "street_name" },
                values: new object[] { null, "Rákóczi Ferenc utca" });

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 6,
                columns: new[] { "building_name", "street_name" },
                values: new object[] { null, "Szabadság tér" });

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 7,
                columns: new[] { "building_name", "street_name" },
                values: new object[] { null, "Zrínyi Ilona u." });
        }
    }
}
