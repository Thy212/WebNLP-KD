using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebLabAI.Migrations
{
    public partial class updateResearch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Researchs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Researchs_ApplicationUserId",
                table: "Researchs",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Researchs_AspNetUsers_ApplicationUserId",
                table: "Researchs",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Researchs_AspNetUsers_ApplicationUserId",
                table: "Researchs");

            migrationBuilder.DropIndex(
                name: "IX_Researchs_ApplicationUserId",
                table: "Researchs");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Researchs");
        }
    }
}
