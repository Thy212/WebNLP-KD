using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebLabAI.Migrations
{
    public partial class ThumbnailUrlAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_applicationUserId",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "applicationUserId",
                table: "Posts",
                newName: "ApplicationUserId");

            migrationBuilder.RenameColumn(
                name: "ApplicationUser",
                table: "Posts",
                newName: "ThumbnailUrl");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_applicationUserId",
                table: "Posts",
                newName: "IX_Posts_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_ApplicationUserId",
                table: "Posts",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_ApplicationUserId",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Posts",
                newName: "applicationUserId");

            migrationBuilder.RenameColumn(
                name: "ThumbnailUrl",
                table: "Posts",
                newName: "ApplicationUser");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_ApplicationUserId",
                table: "Posts",
                newName: "IX_Posts_applicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_applicationUserId",
                table: "Posts",
                column: "applicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
