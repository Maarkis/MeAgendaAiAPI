using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeAgendaAi.Data.Migrations
{
    public partial class ChangeEntityNameFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeService");

            migrationBuilder.CreateTable(
                name: "ServiceEmployee",
                columns: table => new
                {
                    EmployeeServiceId = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    UpdatedBy = table.Column<Guid>(nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    ServiceId = table.Column<Guid>(nullable: false),
                    EmployeeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceEmployee", x => x.EmployeeServiceId);
                    table.ForeignKey(
                        name: "FK_ServiceEmployee_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceEmployee_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceEmployee_EmployeeId",
                table: "ServiceEmployee",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceEmployee_ServiceId",
                table: "ServiceEmployee",
                column: "ServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceEmployee");

            migrationBuilder.CreateTable(
                name: "EmployeeService",
                columns: table => new
                {
                    EmployeeServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000"))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeService", x => x.EmployeeServiceId);
                    table.ForeignKey(
                        name: "FK_EmployeeService_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeService_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeService_EmployeeId",
                table: "EmployeeService",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeService_ServiceId",
                table: "EmployeeService",
                column: "ServiceId");
        }
    }
}
