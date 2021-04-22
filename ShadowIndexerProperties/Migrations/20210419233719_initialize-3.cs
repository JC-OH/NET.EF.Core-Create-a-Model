using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShadowIndexerProperties.Migrations
{
    public partial class initialize3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogImage",
                columns: table => new
                {
                    BlogImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlogId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogImage", x => x.BlogImageId);
                    table.ForeignKey(
                        name: "FK_BlogImage_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "BlogId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogImage_BlogId",
                table: "BlogImage",
                column: "BlogId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogImage");
        }
    }
}
