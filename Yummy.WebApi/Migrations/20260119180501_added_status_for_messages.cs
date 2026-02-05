using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yummy.WebApi.Migrations
{
    public partial class added_status_for_messages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Messages");
        }
    }
}
