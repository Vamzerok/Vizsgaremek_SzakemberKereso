using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SzakemberKereso.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDurationAddJobDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "duration_in_minutes",
                table: "services");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "jobs",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "jobs");

            migrationBuilder.AddColumn<int>(
                name: "duration_in_minutes",
                table: "services",
                type: "int",
                nullable: true);
        }
    }
}
