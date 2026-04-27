using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SzakemberKereso.Migrations
{
    /// <inheritdoc />
    public partial class SeedMockData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "expert_specialties",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "services",
                keyColumn: "service_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "services",
                keyColumn: "service_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "services",
                keyColumn: "service_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "services",
                keyColumn: "service_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "expert_specialties",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "expert_specialties",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "expert_specialties",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "expert_specialties",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.InsertData(
                table: "occupations",
                columns: new[] { "feor_id", "description", "name" },
                values: new object[,]
                {
                    { 7223, "", "Bútorasztalos" },
                    { 7511, "", "Kőműves" },
                    { 7513, "", "Ács" },
                    { 7521, "", "Vezeték- és csőhálózat-szerelő (víz, gáz, fűtés)" },
                    { 7524, "", "Épületvillamossági szerelő, villanyszerelő" },
                    { 7532, "", "Tetőfedő" },
                    { 7534, "", "Burkoló" },
                    { 7535, "", "Festő és mázoló" }
                });

            migrationBuilder.InsertData(
                table: "settlements",
                columns: new[] { "settlement_id", "county_name", "name", "postal_code" },
                values: new object[,]
                {
                    { 1, "Fejér", "Aba", 8127 },
                    { 2, "Jász-Nagykun-Szolnok", "Abádszalók", 5241 },
                    { 3, "Baranya", "Abaliget", 7678 },
                    { 4, "Heves", "Abasár", 3261 },
                    { 5, "Borsod-Abaúj-Zemplén", "Abaújalpár", 3882 },
                    { 6, "Borsod-Abaúj-Zemplén", "Abaújkér", 3882 },
                    { 7, "Borsod-Abaúj-Zemplén", "Abaújlak", 3815 },
                    { 8, "Borsod-Abaúj-Zemplén", "Abaújszántó", 3881 }
                });

            migrationBuilder.InsertData(
                table: "residential_addresses",
                columns: new[] { "residential_address_id", "building_name", "building_number", "door_number", "floor_number", "settlement_id", "street_name" },
                values: new object[,]
                {
                    { 1, null, 12, null, null, 1, "Fő utca" },
                    { 2, null, 3, null, null, 1, "Petőfi utca" },
                    { 3, null, 7, null, null, 2, "Kossuth tér" },
                    { 4, null, 21, null, null, 3, "Dózsa György út" },
                    { 5, null, 5, null, null, 4, "Rákóczi Ferenc utca" },
                    { 6, null, 14, null, null, 5, "Szabadság tér" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "occupations",
                keyColumn: "feor_id",
                keyValue: 7223);

            migrationBuilder.DeleteData(
                table: "occupations",
                keyColumn: "feor_id",
                keyValue: 7511);

            migrationBuilder.DeleteData(
                table: "occupations",
                keyColumn: "feor_id",
                keyValue: 7513);

            migrationBuilder.DeleteData(
                table: "occupations",
                keyColumn: "feor_id",
                keyValue: 7521);

            migrationBuilder.DeleteData(
                table: "occupations",
                keyColumn: "feor_id",
                keyValue: 7524);

            migrationBuilder.DeleteData(
                table: "occupations",
                keyColumn: "feor_id",
                keyValue: 7532);

            migrationBuilder.DeleteData(
                table: "occupations",
                keyColumn: "feor_id",
                keyValue: 7534);

            migrationBuilder.DeleteData(
                table: "occupations",
                keyColumn: "feor_id",
                keyValue: 7535);

            migrationBuilder.DeleteData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "settlements",
                keyColumn: "settlement_id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "settlements",
                keyColumn: "settlement_id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "settlements",
                keyColumn: "settlement_id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "settlements",
                keyColumn: "settlement_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "settlements",
                keyColumn: "settlement_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "settlements",
                keyColumn: "settlement_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "settlements",
                keyColumn: "settlement_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "settlements",
                keyColumn: "settlement_id",
                keyValue: 5);

            migrationBuilder.InsertData(
                table: "expert_specialties",
                columns: new[] { "id", "expert_id", "occupation_id" },
                values: new object[,]
                {
                    { 1, 4, 7521 },
                    { 2, 5, 7524 },
                    { 3, 5, 7223 },
                    { 4, 6, 7223 },
                    { 5, 7, 7535 }
                });

            migrationBuilder.InsertData(
                table: "services",
                columns: new[] { "service_id", "description", "duration_in_minutes", "expert_specialties_id", "fixed_price", "name", "pricing_type", "unit_name", "unit_price" },
                values: new object[,]
                {
                    { 1, "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", 15, 1, 2000m, "Sarokszelep cserélés", 1, "sarokszelep", 1000m },
                    { 2, "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", 20, 2, 3000m, "Mennyezeit lámpa beépítés", 1, "lámpa", 1500m },
                    { 3, "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", 45, 3, 20000m, "Szekrény javítás", 0, null, null },
                    { 4, "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", null, 4, 10000m, "Szoba kifestése", 1, "m2", 1000m }
                });
        }
    }
}
