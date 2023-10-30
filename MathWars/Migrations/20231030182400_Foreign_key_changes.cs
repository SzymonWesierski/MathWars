using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathWars.Migrations
{
    /// <inheritdoc />
    public partial class Foreign_key_changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Users_UsersId",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_UsersId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "Answers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "Answers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answers_UsersId",
                table: "Answers",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Users_UsersId",
                table: "Answers",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
