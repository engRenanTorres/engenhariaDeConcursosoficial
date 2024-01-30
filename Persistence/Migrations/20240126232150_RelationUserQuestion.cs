using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RelationUserQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById1",
                table: "Questions",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CreatedById1",
                table: "Questions",
                column: "CreatedById1");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_AspNetUsers_CreatedById1",
                table: "Questions",
                column: "CreatedById1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_AspNetUsers_CreatedById1",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_CreatedById1",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "CreatedById1",
                table: "Questions");
        }
    }
}
