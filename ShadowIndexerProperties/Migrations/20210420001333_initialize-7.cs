using Microsoft.EntityFrameworkCore.Migrations;

namespace ShadowIndexerProperties.Migrations
{
    public partial class initialize7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorUserId",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AuthorUserId1",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContributorUserId",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ContributorUserId1",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AuthorUserId1",
                table: "Posts",
                column: "AuthorUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ContributorUserId1",
                table: "Posts",
                column: "ContributorUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_User_AuthorUserId1",
                table: "Posts",
                column: "AuthorUserId1",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_User_ContributorUserId1",
                table: "Posts",
                column: "ContributorUserId1",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_User_AuthorUserId1",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_User_ContributorUserId1",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_Posts_AuthorUserId1",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_ContributorUserId1",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "AuthorUserId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "AuthorUserId1",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ContributorUserId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ContributorUserId1",
                table: "Posts");
        }
    }
}
