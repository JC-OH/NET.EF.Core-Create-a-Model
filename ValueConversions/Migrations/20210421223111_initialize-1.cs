using Microsoft.EntityFrameworkCore.Migrations;

namespace ValueConversions.Migrations
{
    public partial class initialize1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rider1s",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rider1s", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rider2s",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mount = table.Column<string>(type: "nvarchar(24)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rider2s", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rider1s");

            migrationBuilder.DropTable(
                name: "Rider2s");
        }
    }
}
