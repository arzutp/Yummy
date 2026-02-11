using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yummy.WebApi.Migrations
{
    public partial class added_newprop_about : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubTitle1",
                table: "Abouts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SubTitle2",
                table: "Abouts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SubTitle3",
                table: "Abouts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubTitle1",
                table: "Abouts");

            migrationBuilder.DropColumn(
                name: "SubTitle2",
                table: "Abouts");

            migrationBuilder.DropColumn(
                name: "SubTitle3",
                table: "Abouts");
        }
    }
}
