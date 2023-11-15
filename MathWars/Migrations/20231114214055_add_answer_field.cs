using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathWars.Migrations
{
    /// <inheritdoc />
    public partial class add_answer_field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Answers");

            migrationBuilder.AddColumn<double>(
                name: "Answer",
                table: "Tasks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Answer",
                table: "Answers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answer",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Answer",
                table: "Answers");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
