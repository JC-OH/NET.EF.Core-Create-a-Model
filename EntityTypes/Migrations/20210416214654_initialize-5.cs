using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityTypes.Migrations
{
    public partial class initialize5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "blogs",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 3m,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AddColumn<string>(
                name: "Created",
                table: "blogs",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "Inserted",
                table: "blogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "blogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true, computedColumnSql: "[LastName] + ', ' + [FirstName]"),
                    NameLength = table.Column<int>(type: "int", nullable: false, computedColumnSql: "LEN([LastName]) + LEN([FirstName])", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.PersonId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "blogs");

            migrationBuilder.DropColumn(
                name: "Inserted",
                table: "blogs");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "blogs");

            migrationBuilder.AlterColumn<decimal>(
                name: "Rating",
                table: "blogs",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldDefaultValue: 3m);
        }
    }
}
