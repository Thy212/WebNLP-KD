﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebLabAI.Migrations
{
    public partial class thumbnailOnUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "AspNetUsers");
        }
    }
}
