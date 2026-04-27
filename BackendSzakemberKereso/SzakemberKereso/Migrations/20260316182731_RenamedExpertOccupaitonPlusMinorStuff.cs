using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SzakemberKereso.Migrations
{
    /// <inheritdoc />
    public partial class RenamedExpertOccupaitonPlusMinorStuff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_studies_occupations_occupation_id",
                table: "studies");

            migrationBuilder.RenameColumn(
                name: "occupation_id",
                table: "studies",
                newName: "OccupationId");

            migrationBuilder.RenameIndex(
                name: "IX_studies_occupation_id",
                table: "studies",
                newName: "IX_studies_OccupationId");

            migrationBuilder.AlterColumn<int>(
                name: "OccupationId",
                table: "studies",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "expert_specialties_id",
                table: "studies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_studies_expert_specialties_id",
                table: "studies",
                column: "expert_specialties_id");

            migrationBuilder.AddForeignKey(
                name: "FK_studies_expert_specialties_expert_specialties_id",
                table: "studies",
                column: "expert_specialties_id",
                principalTable: "expert_specialties",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_studies_occupations_OccupationId",
                table: "studies",
                column: "OccupationId",
                principalTable: "occupations",
                principalColumn: "feor_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_services_expert_specialties_expert_specialties_id",
                table: "services");

            migrationBuilder.DropForeignKey(
                name: "FK_studies_expert_specialties_expert_specialties_id",
                table: "studies");

            migrationBuilder.DropForeignKey(
                name: "FK_studies_occupations_OccupationId",
                table: "studies");

            migrationBuilder.DropIndex(
                name: "IX_studies_expert_specialties_id",
                table: "studies");

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

            migrationBuilder.DropColumn(
                name: "expert_specialties_id",
                table: "studies");

            migrationBuilder.RenameColumn(
                name: "OccupationId",
                table: "studies",
                newName: "occupation_id");

            migrationBuilder.RenameIndex(
                name: "IX_studies_OccupationId",
                table: "studies",
                newName: "IX_studies_occupation_id");

            migrationBuilder.RenameColumn(
                name: "expert_specialties_id",
                table: "services",
                newName: "expert_feor_id");

            migrationBuilder.RenameIndex(
                name: "IX_services_expert_specialties_id",
                table: "services",
                newName: "IX_services_expert_feor_id");

            migrationBuilder.AlterColumn<int>(
                name: "occupation_id",
                table: "studies",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_services_expert_specialties_expert_feor_id",
                table: "services",
                column: "expert_feor_id",
                principalTable: "expert_specialties",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_studies_occupations_occupation_id",
                table: "studies",
                column: "occupation_id",
                principalTable: "occupations",
                principalColumn: "feor_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
