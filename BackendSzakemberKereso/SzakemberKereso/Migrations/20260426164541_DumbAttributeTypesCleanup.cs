using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SzakemberKereso.Migrations
{
    /// <inheritdoc />
    public partial class DumbAttributeTypesCleanup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "door_number",
                table: "residential_addresses");

            migrationBuilder.DropColumn(
                name: "floor_number",
                table: "residential_addresses");

            migrationBuilder.RenameColumn(
                name: "building_name",
                table: "residential_addresses",
                newName: "public_area_type");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "LastName",
                keyValue: null,
                column: "LastName",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "users",
                type: "varchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "FirstName",
                keyValue: null,
                column: "FirstName",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "users",
                type: "varchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "settlements",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "county_name",
                table: "settlements",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "services",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "services",
                type: "varchar(512)",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "street_name",
                table: "residential_addresses",
                type: "varchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "building_number",
                table: "residential_addresses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "public_area_type",
                keyValue: null,
                column: "public_area_type",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "public_area_type",
                table: "residential_addresses",
                type: "varchar(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "work_location_id",
                table: "experts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "experts",
                keyColumn: "company_phone_number",
                keyValue: null,
                column: "company_phone_number",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "company_phone_number",
                table: "experts",
                type: "varchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(32)",
                oldMaxLength: 32,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "experts",
                keyColumn: "company_email",
                keyValue: null,
                column: "company_email",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "company_email",
                table: "experts",
                type: "varchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldMaxLength: 256,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "public_area_type",
                table: "residential_addresses",
                newName: "building_name");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "users",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(32)",
                oldMaxLength: 32)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "users",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(32)",
                oldMaxLength: 32)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "settlements",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "county_name",
                table: "settlements",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "services",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "services",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(512)",
                oldMaxLength: 512,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "street_name",
                table: "residential_addresses",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldMaxLength: 150)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "building_number",
                table: "residential_addresses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "building_name",
                table: "residential_addresses",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(16)",
                oldMaxLength: 16)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "door_number",
                table: "residential_addresses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "floor_number",
                table: "residential_addresses",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "work_location_id",
                table: "experts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "company_phone_number",
                table: "experts",
                type: "varchar(32)",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(32)",
                oldMaxLength: 32)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "company_email",
                table: "experts",
                type: "varchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldMaxLength: 256)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 1,
                columns: new[] { "door_number", "floor_number" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 2,
                columns: new[] { "door_number", "floor_number" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 3,
                columns: new[] { "door_number", "floor_number" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 4,
                columns: new[] { "door_number", "floor_number" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 5,
                columns: new[] { "door_number", "floor_number" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 6,
                columns: new[] { "door_number", "floor_number" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 7,
                columns: new[] { "door_number", "floor_number" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 8,
                columns: new[] { "door_number", "floor_number" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 9,
                columns: new[] { "door_number", "floor_number" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 10,
                columns: new[] { "door_number", "floor_number" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 11,
                columns: new[] { "door_number", "floor_number" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 12,
                columns: new[] { "door_number", "floor_number" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 13,
                columns: new[] { "door_number", "floor_number" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 14,
                columns: new[] { "door_number", "floor_number" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "residential_addresses",
                keyColumn: "residential_address_id",
                keyValue: 15,
                columns: new[] { "door_number", "floor_number" },
                values: new object[] { null, null });
        }
    }
}
