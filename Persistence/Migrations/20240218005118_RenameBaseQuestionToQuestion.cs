using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameBaseQuestionToQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Choices_Questions_BaseQuestionId",
                table: "Choices");

            migrationBuilder.RenameColumn(
                name: "BaseQuestionId",
                table: "Choices",
                newName: "QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Choices_BaseQuestionId",
                table: "Choices",
                newName: "IX_Choices_QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Choices_Questions_QuestionId",
                table: "Choices",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Choices_Questions_QuestionId",
                table: "Choices");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "Choices",
                newName: "BaseQuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Choices_QuestionId",
                table: "Choices",
                newName: "IX_Choices_BaseQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Choices_Questions_BaseQuestionId",
                table: "Choices",
                column: "BaseQuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
