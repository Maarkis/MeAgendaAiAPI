using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeAgendaAi.Data.Migrations
{
    public partial class ChangeTableEmployeeServiceName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "User",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 263, DateTimeKind.Unspecified).AddTicks(962),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 174, DateTimeKind.Unspecified).AddTicks(8363));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "User",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 263, DateTimeKind.Unspecified).AddTicks(643),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 174, DateTimeKind.Unspecified).AddTicks(8046));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "Service",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 257, DateTimeKind.Unspecified).AddTicks(2691),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 168, DateTimeKind.Unspecified).AddTicks(8041));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Service",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 257, DateTimeKind.Unspecified).AddTicks(2394),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 168, DateTimeKind.Unspecified).AddTicks(7769));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "Scheduling",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 254, DateTimeKind.Unspecified).AddTicks(1673),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 165, DateTimeKind.Unspecified).AddTicks(8169));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Scheduling",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 254, DateTimeKind.Unspecified).AddTicks(1420),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 165, DateTimeKind.Unspecified).AddTicks(7963));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "Policy",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 251, DateTimeKind.Unspecified).AddTicks(7897),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 163, DateTimeKind.Unspecified).AddTicks(3808));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Policy",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 251, DateTimeKind.Unspecified).AddTicks(7623),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 163, DateTimeKind.Unspecified).AddTicks(3519));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "EmployeeService",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 247, DateTimeKind.Unspecified).AddTicks(8506),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 158, DateTimeKind.Unspecified).AddTicks(5217));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "EmployeeService",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 247, DateTimeKind.Unspecified).AddTicks(8214),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 158, DateTimeKind.Unspecified).AddTicks(4724));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "Employee",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 245, DateTimeKind.Unspecified).AddTicks(7083),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 155, DateTimeKind.Unspecified).AddTicks(1295));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Employee",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 245, DateTimeKind.Unspecified).AddTicks(6807),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 155, DateTimeKind.Unspecified).AddTicks(748));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "Company",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 241, DateTimeKind.Unspecified).AddTicks(3853),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 146, DateTimeKind.Unspecified).AddTicks(2238));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Company",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 241, DateTimeKind.Unspecified).AddTicks(3572),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 146, DateTimeKind.Unspecified).AddTicks(1548));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "Client",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 239, DateTimeKind.Unspecified).AddTicks(9199),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 142, DateTimeKind.Unspecified).AddTicks(8629));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Client",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 235, DateTimeKind.Unspecified).AddTicks(1946),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 125, DateTimeKind.Unspecified).AddTicks(1504));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 174, DateTimeKind.Unspecified).AddTicks(8363),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 263, DateTimeKind.Unspecified).AddTicks(962));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 174, DateTimeKind.Unspecified).AddTicks(8046),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 263, DateTimeKind.Unspecified).AddTicks(643));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "Service",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 168, DateTimeKind.Unspecified).AddTicks(8041),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 257, DateTimeKind.Unspecified).AddTicks(2691));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Service",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 168, DateTimeKind.Unspecified).AddTicks(7769),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 257, DateTimeKind.Unspecified).AddTicks(2394));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "Scheduling",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 165, DateTimeKind.Unspecified).AddTicks(8169),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 254, DateTimeKind.Unspecified).AddTicks(1673));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Scheduling",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 165, DateTimeKind.Unspecified).AddTicks(7963),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 254, DateTimeKind.Unspecified).AddTicks(1420));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "Policy",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 163, DateTimeKind.Unspecified).AddTicks(3808),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 251, DateTimeKind.Unspecified).AddTicks(7897));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Policy",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 163, DateTimeKind.Unspecified).AddTicks(3519),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 251, DateTimeKind.Unspecified).AddTicks(7623));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "EmployeeService",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 158, DateTimeKind.Unspecified).AddTicks(5217),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 247, DateTimeKind.Unspecified).AddTicks(8506));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "EmployeeService",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 158, DateTimeKind.Unspecified).AddTicks(4724),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 247, DateTimeKind.Unspecified).AddTicks(8214));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "Employee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 155, DateTimeKind.Unspecified).AddTicks(1295),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 245, DateTimeKind.Unspecified).AddTicks(7083));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Employee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 155, DateTimeKind.Unspecified).AddTicks(748),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 245, DateTimeKind.Unspecified).AddTicks(6807));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "Company",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 146, DateTimeKind.Unspecified).AddTicks(2238),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 241, DateTimeKind.Unspecified).AddTicks(3853));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Company",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 146, DateTimeKind.Unspecified).AddTicks(1548),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 241, DateTimeKind.Unspecified).AddTicks(3572));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "Client",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 142, DateTimeKind.Unspecified).AddTicks(8629),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 239, DateTimeKind.Unspecified).AddTicks(9199));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Client",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 9, 26, 19, 56, 8, 125, DateTimeKind.Unspecified).AddTicks(1504),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 9, 26, 20, 23, 59, 235, DateTimeKind.Unspecified).AddTicks(1946));
        }
    }
}
