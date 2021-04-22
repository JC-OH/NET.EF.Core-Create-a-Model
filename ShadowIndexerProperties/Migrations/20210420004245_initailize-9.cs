using Microsoft.EntityFrameworkCore.Migrations;

namespace ShadowIndexerProperties.Migrations
{
    public partial class initailize9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Blogs",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RssUrl",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "RssUrl",
                table: "Blogs");
        }
    }
}
