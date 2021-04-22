using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityTypes.Migrations
{
    public partial class initialize3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "blogs",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                comment: "The URL of the Blog",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Score",
                table: "blogs",
                type: "decimal(14,2)",
                precision: 14,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "blogs",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldComment: "The URL of the Blog");

            migrationBuilder.AlterColumn<decimal>(
                name: "Score",
                table: "blogs",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(14,2)",
                oldPrecision: 14,
                oldScale: 2);
        }
    }
}
