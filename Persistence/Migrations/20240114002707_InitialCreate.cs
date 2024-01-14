using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Choices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Letter = table.Column<char>(type: "character(1)", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    BaseQuestionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Choices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Last_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    Answer = table.Column<char>(type: "character(1)", nullable: false),
                    Tip = table.Column<string>(type: "text", nullable: true),
                    CreatedById = table.Column<int>(type: "integer", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(34)", maxLength: 34, nullable: false),
                    ChoiceAId = table.Column<int>(type: "integer", nullable: true),
                    ChoiceBId = table.Column<int>(type: "integer", nullable: true),
                    ChoiceCId = table.Column<int>(type: "integer", nullable: true),
                    ChoiceDId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Choices_ChoiceAId",
                        column: x => x.ChoiceAId,
                        principalTable: "Choices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Questions_Choices_ChoiceBId",
                        column: x => x.ChoiceBId,
                        principalTable: "Choices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Questions_Choices_ChoiceCId",
                        column: x => x.ChoiceCId,
                        principalTable: "Choices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Questions_Choices_ChoiceDId",
                        column: x => x.ChoiceDId,
                        principalTable: "Choices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Choices_BaseQuestionId",
                table: "Choices",
                column: "BaseQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ChoiceAId",
                table: "Questions",
                column: "ChoiceAId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ChoiceBId",
                table: "Questions",
                column: "ChoiceBId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ChoiceCId",
                table: "Questions",
                column: "ChoiceCId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ChoiceDId",
                table: "Questions",
                column: "ChoiceDId");

            migrationBuilder.AddForeignKey(
                name: "FK_Choices_Questions_BaseQuestionId",
                table: "Choices",
                column: "BaseQuestionId",
                principalTable: "Questions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Choices_Questions_BaseQuestionId",
                table: "Choices");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Choices");
        }
    }
}
