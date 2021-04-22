using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityTypes.Migrations
{
    public partial class initialize2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "blogs",
                comment: "Blogs managed on the web site.");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "blogs",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Rating",
                table: "blogs",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Score",
                table: "blogs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "blogs");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "blogs");

            migrationBuilder.AlterTable(
                name: "blogs",
                oldComment: "Blogs managed on the web site.");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "blogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true);
        }
    }
}
