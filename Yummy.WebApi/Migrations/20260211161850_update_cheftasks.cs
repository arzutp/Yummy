using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yummy.WebApi.Migrations
{
    public partial class update_cheftasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ChefEmployeeTasks",
                table: "ChefEmployeeTasks");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ChefEmployeeTasks",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChefEmployeeTasks",
                table: "ChefEmployeeTasks",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChefEmployeeTasks_ChefId",
                table: "ChefEmployeeTasks",
                column: "ChefId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ChefEmployeeTasks",
                table: "ChefEmployeeTasks");

            migrationBuilder.DropIndex(
                name: "IX_ChefEmployeeTasks_ChefId",
                table: "ChefEmployeeTasks");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ChefEmployeeTasks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChefEmployeeTasks",
                table: "ChefEmployeeTasks",
                columns: new[] { "ChefId", "EmployeeTaskId" });
        }
    }
}
