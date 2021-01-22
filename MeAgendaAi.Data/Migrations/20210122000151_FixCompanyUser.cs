using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeAgendaAi.Data.Migrations
{
    public partial class FixCompanyUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_User_UserId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "CPF",
                table: "User");

            migrationBuilder.DropColumn(
                name: "RG",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CPF",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Company");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Location",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CPF",
                table: "Employee",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "Employee",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RG",
                table: "Employee",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "Company",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Company",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "CPF",
                table: "Client",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RG",
                table: "Client",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_User_UserId",
                table: "Location",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_User_UserId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "CPF",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "RG",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "CPF",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "RG",
                table: "Client");

            migrationBuilder.AddColumn<string>(
                name: "CPF",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RG",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Location",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<string>(
                name: "CPF",
                table: "Company",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Company",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Location_User_UserId",
                table: "Location",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
