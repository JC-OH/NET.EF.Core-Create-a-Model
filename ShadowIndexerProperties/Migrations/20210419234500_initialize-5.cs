using Microsoft.EntityFrameworkCore.Migrations;

namespace ShadowIndexerProperties.Migrations
{
    public partial class initialize5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostTag_Tag_TagsTagId",
                table: "PostTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tag",
                table: "Tag");

            migrationBuilder.RenameTable(
                name: "Tag",
                newName: "Tags");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostTag_Tags_TagsTagId",
                table: "PostTag",
                column: "TagsTagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostTag_Tags_TagsTagId",
                table: "PostTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "Tag");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tag",
                table: "Tag",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostTag_Tag_TagsTagId",
                table: "PostTag",
                column: "TagsTagId",
                principalTable: "Tag",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
