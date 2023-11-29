using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathWars.Migrations
{
    /// <inheritdoc />
    public partial class ralationManyToOneBetweenTaskAndTaskCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TasksCategory_categoryId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "categoryId",
                table: "Tasks",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_categoryId",
                table: "Tasks",
                newName: "IX_Tasks_CategoryId");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TasksCategory_CategoryId",
                table: "Tasks",
                column: "CategoryId",
                principalTable: "TasksCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TasksCategory_CategoryId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Tasks",
                newName: "categoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_CategoryId",
                table: "Tasks",
                newName: "IX_Tasks_categoryId");

            migrationBuilder.AlterColumn<int>(
                name: "categoryId",
                table: "Tasks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TasksCategory_categoryId",
                table: "Tasks",
                column: "categoryId",
                principalTable: "TasksCategory",
                principalColumn: "Id");
        }
    }
}
