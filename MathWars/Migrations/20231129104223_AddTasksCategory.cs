using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathWars.Migrations
{
    /// <inheritdoc />
    public partial class AddTasksCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "category",
                table: "Tasks");

            migrationBuilder.AddColumn<int>(
                name: "categoryId",
                table: "Tasks",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TasksCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TasksCategory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_categoryId",
                table: "Tasks",
                column: "categoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TasksCategory_categoryId",
                table: "Tasks",
                column: "categoryId",
                principalTable: "TasksCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TasksCategory_categoryId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "TasksCategory");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_categoryId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "categoryId",
                table: "Tasks");

            migrationBuilder.AddColumn<string>(
                name: "category",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
