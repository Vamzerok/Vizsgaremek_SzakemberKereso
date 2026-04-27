using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SzakemberKereso.Migrations
{
    /// <inheritdoc />
    public partial class AddExpertCompanyInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "company_email",
                table: "experts",
                type: "varchar(256)",
                maxLength: 256,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "company_phone_number",
                table: "experts",
                type: "varchar(32)",
                maxLength: 32,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "company_email",
                table: "experts");

            migrationBuilder.DropColumn(
                name: "company_phone_number",
                table: "experts");
        }
    }
}
