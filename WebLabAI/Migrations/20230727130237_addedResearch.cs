using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebLabAI.Migrations
{
    public partial class addedResearch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Pages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Researchs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Researchs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pages_ApplicationUserId",
                table: "Pages",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pages_AspNetUsers_ApplicationUserId",
                table: "Pages",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pages_AspNetUsers_ApplicationUserId",
                table: "Pages");

            migrationBuilder.DropTable(
                name: "Researchs");

            migrationBuilder.DropIndex(
                name: "IX_Pages_ApplicationUserId",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Pages");
        }
    }
}
