using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityTypes.Migrations
{
    public partial class initialize1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Blogs_BlogId",
                table: "Post");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Blogs",
                table: "Blogs");

            migrationBuilder.RenameTable(
                name: "Blogs",
                newName: "blogs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_blogs",
                table: "blogs",
                column: "BlogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_blogs_BlogId",
                table: "Post",
                column: "BlogId",
                principalTable: "blogs",
                principalColumn: "BlogId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_blogs_BlogId",
                table: "Post");

            migrationBuilder.DropPrimaryKey(
                name: "PK_blogs",
                table: "blogs");

            migrationBuilder.RenameTable(
                name: "blogs",
                newName: "Blogs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Blogs",
                table: "Blogs",
                column: "BlogId");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Blogs_BlogId",
                table: "Post",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "BlogId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
