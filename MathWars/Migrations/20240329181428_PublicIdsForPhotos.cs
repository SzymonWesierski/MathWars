using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathWars.Migrations
{
    /// <inheritdoc />
    public partial class PublicIdsForPhotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicImageId",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicProfileImageId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicPhotoId",
                table: "AnswersToTasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicWhiteBoardPhotoId",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhiteBoardPhotoUrl",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicImageId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "PublicProfileImageId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PublicPhotoId",
                table: "AnswersToTasks");

            migrationBuilder.DropColumn(
                name: "PublicWhiteBoardPhotoId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "WhiteBoardPhotoUrl",
                table: "Answers");
        }
    }
}
