using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SzakemberKereso.Migrations
{
    /// <inheritdoc />
    public partial class FixedStudyRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_studies_occupations_OccupationId",
                table: "studies");

            migrationBuilder.DropIndex(
                name: "IX_studies_OccupationId",
                table: "studies");

            migrationBuilder.DropColumn(
                name: "OccupationId",
                table: "studies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OccupationId",
                table: "studies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_studies_OccupationId",
                table: "studies",
                column: "OccupationId");

            migrationBuilder.AddForeignKey(
                name: "FK_studies_occupations_OccupationId",
                table: "studies",
                column: "OccupationId",
                principalTable: "occupations",
                principalColumn: "feor_id");
        }
    }
}
