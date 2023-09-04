using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebLabAI.Migrations
{
    public partial class updateTraining2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Trainings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Trainings");
        }
    }
}
