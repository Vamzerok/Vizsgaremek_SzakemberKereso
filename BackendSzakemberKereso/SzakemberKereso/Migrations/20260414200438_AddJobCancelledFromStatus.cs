using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SzakemberKereso.Migrations
{
    /// <inheritdoc />
    public partial class AddJobCancelledFromStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "cancelled_from_status",
                table: "jobs",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cancelled_from_status",
                table: "jobs");
        }
    }
}
