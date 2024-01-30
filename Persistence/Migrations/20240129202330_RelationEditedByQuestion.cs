using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RelationEditedByQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_AspNetUsers_CreatedById1",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "CreatedById1",
                table: "Questions",
                newName: "EditedById");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_CreatedById1",
                table: "Questions",
                newName: "IX_Questions_EditedById");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "Questions",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CreatedById",
                table: "Questions",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_AspNetUsers_CreatedById",
                table: "Questions",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_AspNetUsers_EditedById",
                table: "Questions",
                column: "EditedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_AspNetUsers_CreatedById",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_AspNetUsers_EditedById",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_CreatedById",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "EditedById",
                table: "Questions",
                newName: "CreatedById1");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_EditedById",
                table: "Questions",
                newName: "IX_Questions_CreatedById1");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "Questions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_AspNetUsers_CreatedById1",
                table: "Questions",
                column: "CreatedById1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
