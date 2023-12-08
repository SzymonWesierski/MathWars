using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MathWars.Migrations
{
    /// <inheritdoc />
    public partial class AddAnswersTypeAndRelationManyToManyBeatweenTaskAndCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TasksCategory_categoryId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_categoryId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "categoryId",
                table: "Tasks");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "TasksCategory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "AnswerTypeId",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Answers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "AnswerTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormatExplanation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HowManyCorrectAnswers = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TasksAndCategories",
                columns: table => new
                {
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    TaskCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TasksAndCategories", x => new { x.TaskCategoryId, x.TaskId });
                    table.ForeignKey(
                        name: "FK_TasksAndCategories_TasksCategory_TaskCategoryId",
                        column: x => x.TaskCategoryId,
                        principalTable: "TasksCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TasksAndCategories_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AnswerTypeId",
                table: "Tasks",
                column: "AnswerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TasksAndCategories_TaskId",
                table: "TasksAndCategories",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AnswerTypes_AnswerTypeId",
                table: "Tasks",
                column: "AnswerTypeId",
                principalTable: "AnswerTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AnswerTypes_AnswerTypeId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "AnswerTypes");

            migrationBuilder.DropTable(
                name: "TasksAndCategories");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_AnswerTypeId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "TasksCategory");

            migrationBuilder.DropColumn(
                name: "AnswerTypeId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Answers");

            migrationBuilder.AlterColumn<double>(
                name: "Answer",
                table: "Tasks",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "categoryId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Answer",
                table: "Answers",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_categoryId",
                table: "Tasks",
                column: "categoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TasksCategory_categoryId",
                table: "Tasks",
                column: "categoryId",
                principalTable: "TasksCategory",
                principalColumn: "Id");
        }
    }
}
