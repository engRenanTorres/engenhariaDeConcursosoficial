using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FirstCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    Discriminator = table.Column<string>(type: "character varying(34)", maxLength: 34, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                });

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
                    table.ForeignKey(
                        name: "FK_Choices_Questions_BaseQuestionId",
                        column: x => x.BaseQuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Choices_BaseQuestionId",
                table: "Choices",
                column: "BaseQuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Choices");

            migrationBuilder.DropTable(
                name: "Questions");
        }
    }
}
