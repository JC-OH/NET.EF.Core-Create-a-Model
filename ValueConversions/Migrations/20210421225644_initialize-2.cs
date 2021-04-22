using Microsoft.EntityFrameworkCore.Migrations;

namespace ValueConversions.Migrations
{
    public partial class initialize2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsActive",
                table: "Riders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IsActive",
                table: "Rider1s",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Rider3s",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mount = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rider3s", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rider3s");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Riders");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Rider1s");
        }
    }
}
