using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SzakemberKereso.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkLocationToExpert : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "work_location_id",
                table: "experts",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "settlements",
                columns: new[] { "settlement_id", "county_name", "name", "postal_code" },
                values: new object[,]
                {
                    { 10, "Vas", "Sárvár", 9600 },
                    { 11, "Vas", "Ják", 9798 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_experts_work_location_id",
                table: "experts",
                column: "work_location_id");

            migrationBuilder.AddForeignKey(
                name: "FK_experts_settlements_work_location_id",
                table: "experts",
                column: "work_location_id",
                principalTable: "settlements",
                principalColumn: "settlement_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_experts_settlements_work_location_id",
                table: "experts");

            migrationBuilder.DropIndex(
                name: "IX_experts_work_location_id",
                table: "experts");

            migrationBuilder.DeleteData(
                table: "settlements",
                keyColumn: "settlement_id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "settlements",
                keyColumn: "settlement_id",
                keyValue: 11);

            migrationBuilder.DropColumn(
                name: "work_location_id",
                table: "experts");
        }
    }
}
