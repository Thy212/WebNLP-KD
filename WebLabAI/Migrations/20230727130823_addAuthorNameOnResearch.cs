using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebLabAI.Migrations
{
    public partial class addAuthorNameOnResearch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorName",
                table: "Researchs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorName",
                table: "Researchs");
        }
    }
}
