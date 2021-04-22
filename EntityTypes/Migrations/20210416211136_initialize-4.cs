using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityTypes.Migrations
{
    public partial class initialize4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_blogs_BlogId",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Post_BlogId",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "BlogId",
                table: "Post");

            migrationBuilder.AddColumn<string>(
                name: "BlogUrl",
                table: "Post",
                type: "varchar(200)",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_blogs_Url",
                table: "blogs",
                column: "Url");

            migrationBuilder.CreateIndex(
                name: "IX_Post_BlogUrl",
                table: "Post",
                column: "BlogUrl");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_blogs_BlogUrl",
                table: "Post",
                column: "BlogUrl",
                principalTable: "blogs",
                principalColumn: "Url",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_blogs_BlogUrl",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Post_BlogUrl",
                table: "Post");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_blogs_Url",
                table: "blogs");

            migrationBuilder.DropColumn(
                name: "BlogUrl",
                table: "Post");

            migrationBuilder.AddColumn<int>(
                name: "BlogId",
                table: "Post",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Post_BlogId",
                table: "Post",
                column: "BlogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_blogs_BlogId",
                table: "Post",
                column: "BlogId",
                principalTable: "blogs",
                principalColumn: "BlogId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
