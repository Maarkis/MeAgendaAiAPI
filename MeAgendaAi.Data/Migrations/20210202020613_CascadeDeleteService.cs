using Microsoft.EntityFrameworkCore.Migrations;

namespace MeAgendaAi.Data.Migrations
{
    public partial class CascadeDeleteService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceEmployee_Employee_EmployeeId",
                table: "ServiceEmployee");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceEmployee_Service_ServiceId",
                table: "ServiceEmployee");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceEmployee_Employee_EmployeeId",
                table: "ServiceEmployee",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceEmployee_Service_ServiceId",
                table: "ServiceEmployee",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceEmployee_Employee_EmployeeId",
                table: "ServiceEmployee");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceEmployee_Service_ServiceId",
                table: "ServiceEmployee");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceEmployee_Employee_EmployeeId",
                table: "ServiceEmployee",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceEmployee_Service_ServiceId",
                table: "ServiceEmployee",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
