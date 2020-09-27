using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeAgendaAi.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    CompanyId = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    UpdatedBy = table.Column<Guid>(nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    Name = table.Column<string>(nullable: true),
                    CPF = table.Column<string>(nullable: true),
                    CNPJ = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    UpdatedBy = table.Column<Guid>(nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    Email = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 60, nullable: false),
                    Image = table.Column<string>(nullable: true),
                    CPF = table.Column<string>(nullable: false),
                    RG = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Policy",
                columns: table => new
                {
                    PolicyId = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    UpdatedBy = table.Column<Guid>(nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    CompanyId = table.Column<Guid>(nullable: false),
                    LimitCancelHours = table.Column<int>(nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policy", x => x.PolicyId);
                    table.ForeignKey(
                        name: "FK_Policy_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    ServiceId = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    UpdatedBy = table.Column<Guid>(nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    CompanyId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    DurationMinutes = table.Column<int>(nullable: false, defaultValue: 30)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.ServiceId);
                    table.ForeignKey(
                        name: "FK_Service_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    ClientId = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    UpdatedBy = table.Column<Guid>(nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.ClientId);
                    table.ForeignKey(
                        name: "FK_Client_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    UpdatedBy = table.Column<Guid>(nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    UserId = table.Column<Guid>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: false),
                    IsManager = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employee_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employee_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Scheduling",
                columns: table => new
                {
                    SchedulingId = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                    UpdatedBy = table.Column<Guid>(nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    ClientId = table.Column<Guid>(nullable: false),
                    EmployeeId = table.Column<Guid>(nullable: false),
                    ServiceId = table.Column<Guid>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: false, defaultValue: "Scheduled")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scheduling", x => x.SchedulingId);
                    table.ForeignKey(
                        name: "FK_Scheduling_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scheduling_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scheduling_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_Client_UserId",
                table: "Client",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_CompanyId",
                table: "Employee",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_UserId",
                table: "Employee",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Policy_CompanyId",
                table: "Policy",
                column: "CompanyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scheduling_ClientId",
                table: "Scheduling",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Scheduling_EmployeeId",
                table: "Scheduling",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Scheduling_ServiceId",
                table: "Scheduling",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_CompanyId",
                table: "Service",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceEmployee_EmployeeId",
                table: "ServiceEmployee",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceEmployee_ServiceId",
                table: "ServiceEmployee",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Policy");

            migrationBuilder.DropTable(
                name: "Scheduling");

            migrationBuilder.DropTable(
                name: "ServiceEmployee");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Company");
        }
    }
}
