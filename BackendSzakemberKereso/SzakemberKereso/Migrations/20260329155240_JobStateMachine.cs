using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SzakemberKereso.Migrations
{
    /// <inheritdoc />
    public partial class JobStateMachine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_jobs_experts_expert_id",
                table: "jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_jobs_job_states_state_id",
                table: "jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_time_intervals_jobs_JobId",
                table: "time_intervals");

            migrationBuilder.DropTable(
                name: "job_states");

            migrationBuilder.DropIndex(
                name: "IX_jobs_state_id",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "fixed_price",
                table: "services");

            migrationBuilder.DropColumn(
                name: "unit_name",
                table: "services");

            migrationBuilder.DropColumn(
                name: "unit_price",
                table: "services");

            migrationBuilder.DropColumn(
                name: "detail_id",
                table: "jobs");

            migrationBuilder.RenameColumn(
                name: "JobId",
                table: "time_intervals",
                newName: "job_id");

            migrationBuilder.RenameIndex(
                name: "IX_time_intervals_JobId",
                table: "time_intervals",
                newName: "IX_time_intervals_job_id");

            migrationBuilder.RenameColumn(
                name: "pricing_type",
                table: "services",
                newName: "pricing_id");

            migrationBuilder.RenameColumn(
                name: "state_id",
                table: "jobs",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "labor_fee",
                table: "jobs",
                newName: "service_id");

            migrationBuilder.RenameColumn(
                name: "expert_id",
                table: "jobs",
                newName: "pricing_id");

            migrationBuilder.RenameIndex(
                name: "IX_jobs_expert_id",
                table: "jobs",
                newName: "IX_jobs_pricing_id");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "date",
                table: "time_intervals",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "time_intervals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "rating",
                table: "jobs",
                type: "float",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "pricing",
                columns: table => new
                {
                    pricing_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    pricing_type = table.Column<int>(type: "int", nullable: false),
                    fixed_price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    unit_price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    unit_name = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pricing", x => x.pricing_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_services_pricing_id",
                table: "services",
                column: "pricing_id");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_service_id",
                table: "jobs",
                column: "service_id");

            migrationBuilder.AddForeignKey(
                name: "FK_jobs_pricing_pricing_id",
                table: "jobs",
                column: "pricing_id",
                principalTable: "pricing",
                principalColumn: "pricing_id");

            migrationBuilder.AddForeignKey(
                name: "FK_jobs_services_service_id",
                table: "jobs",
                column: "service_id",
                principalTable: "services",
                principalColumn: "service_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_services_pricing_pricing_id",
                table: "services",
                column: "pricing_id",
                principalTable: "pricing",
                principalColumn: "pricing_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_time_intervals_jobs_job_id",
                table: "time_intervals",
                column: "job_id",
                principalTable: "jobs",
                principalColumn: "job_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_jobs_pricing_pricing_id",
                table: "jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_jobs_services_service_id",
                table: "jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_services_pricing_pricing_id",
                table: "services");

            migrationBuilder.DropForeignKey(
                name: "FK_time_intervals_jobs_job_id",
                table: "time_intervals");

            migrationBuilder.DropTable(
                name: "pricing");

            migrationBuilder.DropIndex(
                name: "IX_services_pricing_id",
                table: "services");

            migrationBuilder.DropIndex(
                name: "IX_jobs_service_id",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "type",
                table: "time_intervals");

            migrationBuilder.DropColumn(
                name: "rating",
                table: "jobs");

            migrationBuilder.RenameColumn(
                name: "job_id",
                table: "time_intervals",
                newName: "JobId");

            migrationBuilder.RenameIndex(
                name: "IX_time_intervals_job_id",
                table: "time_intervals",
                newName: "IX_time_intervals_JobId");

            migrationBuilder.RenameColumn(
                name: "pricing_id",
                table: "services",
                newName: "pricing_type");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "jobs",
                newName: "state_id");

            migrationBuilder.RenameColumn(
                name: "service_id",
                table: "jobs",
                newName: "labor_fee");

            migrationBuilder.RenameColumn(
                name: "pricing_id",
                table: "jobs",
                newName: "expert_id");

            migrationBuilder.RenameIndex(
                name: "IX_jobs_pricing_id",
                table: "jobs",
                newName: "IX_jobs_expert_id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date",
                table: "time_intervals",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<decimal>(
                name: "fixed_price",
                table: "services",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

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

            migrationBuilder.AddColumn<int>(
                name: "detail_id",
                table: "jobs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "job_states",
                columns: table => new
                {
                    job_state_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_states", x => x.job_state_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_state_id",
                table: "jobs",
                column: "state_id");

            migrationBuilder.AddForeignKey(
                name: "FK_jobs_experts_expert_id",
                table: "jobs",
                column: "expert_id",
                principalTable: "experts",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_jobs_job_states_state_id",
                table: "jobs",
                column: "state_id",
                principalTable: "job_states",
                principalColumn: "job_state_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_time_intervals_jobs_JobId",
                table: "time_intervals",
                column: "JobId",
                principalTable: "jobs",
                principalColumn: "job_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
