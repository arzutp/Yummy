using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yummy.WebApi.Migrations
{
    public partial class added_chef_employee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChefEmployeeTasks",
                columns: table => new
                {
                    ChefId = table.Column<int>(type: "int", nullable: false),
                    EmployeeTaskId = table.Column<int>(type: "int", nullable: false),
                    AssignDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TaskStatus = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChefEmployeeTasks", x => new { x.ChefId, x.EmployeeTaskId });
                    table.ForeignKey(
                        name: "FK_ChefEmployeeTasks_Chefs_ChefId",
                        column: x => x.ChefId,
                        principalTable: "Chefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChefEmployeeTasks_EmployeeTasks_EmployeeTaskId",
                        column: x => x.EmployeeTaskId,
                        principalTable: "EmployeeTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChefEmployeeTasks_EmployeeTaskId",
                table: "ChefEmployeeTasks",
                column: "EmployeeTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChefEmployeeTasks");

            migrationBuilder.DropTable(
                name: "EmployeeTasks");
        }
    }
}
