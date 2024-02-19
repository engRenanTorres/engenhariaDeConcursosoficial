using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscursiveAndChoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Choices_Questions_QuestionId",
                table: "Choices");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_AspNetUsers_CreatedById",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "Last_updated_at",
                table: "Questions",
                newName: "LastUpdatedAt");

            migrationBuilder.RenameColumn(
                name: "Created_at",
                table: "Questions",
                newName: "InsertedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "Questions",
                newName: "InsertedById");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_CreatedById",
                table: "Questions",
                newName: "IX_Questions_InsertedById");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "Choices",
                newName: "ChoicesQuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Choices_QuestionId",
                table: "Choices",
                newName: "IX_Choices_ChoicesQuestionId");

            migrationBuilder.AlterColumn<char>(
                name: "Answer",
                table: "Questions",
                type: "character(1)",
                nullable: true,
                oldClrType: typeof(char),
                oldType: "character(1)");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Questions",
                type: "character varying(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Choices_Questions_ChoicesQuestionId",
                table: "Choices",
                column: "ChoicesQuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_AspNetUsers_InsertedById",
                table: "Questions",
                column: "InsertedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Choices_Questions_ChoicesQuestionId",
                table: "Choices");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_AspNetUsers_InsertedById",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedAt",
                table: "Questions",
                newName: "Last_updated_at");

            migrationBuilder.RenameColumn(
                name: "InsertedById",
                table: "Questions",
                newName: "CreatedById");

            migrationBuilder.RenameColumn(
                name: "InsertedAt",
                table: "Questions",
                newName: "Created_at");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_InsertedById",
                table: "Questions",
                newName: "IX_Questions_CreatedById");

            migrationBuilder.RenameColumn(
                name: "ChoicesQuestionId",
                table: "Choices",
                newName: "QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Choices_ChoicesQuestionId",
                table: "Choices",
                newName: "IX_Choices_QuestionId");

            migrationBuilder.AlterColumn<char>(
                name: "Answer",
                table: "Questions",
                type: "character(1)",
                nullable: false,
                defaultValue: '\0',
                oldClrType: typeof(char),
                oldType: "character(1)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Choices_Questions_QuestionId",
                table: "Choices",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_AspNetUsers_CreatedById",
                table: "Questions",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
