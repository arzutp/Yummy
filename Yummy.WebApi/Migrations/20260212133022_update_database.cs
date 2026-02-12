using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yummy.WebApi.Migrations
{
    public partial class update_database : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "ChefEmployeeTasks");

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "EmployeeTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "EmployeeTasks");

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "ChefEmployeeTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
