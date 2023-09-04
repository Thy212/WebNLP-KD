using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebLabAI.Migrations
{
    public partial class addManyOnTraining : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Assignment",
                table: "Trainings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Certificate",
                table: "Trainings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Online",
                table: "Trainings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Resources",
                table: "Trainings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Teacher",
                table: "Trainings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "VideoRecord",
                table: "Trainings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Assignment",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "Certificate",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "Online",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "Resources",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "Teacher",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "VideoRecord",
                table: "Trainings");
        }
    }
}
