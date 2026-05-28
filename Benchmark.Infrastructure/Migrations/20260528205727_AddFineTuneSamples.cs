using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Benchmark.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFineTuneSamples : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FineTuneSamples",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProblemId = table.Column<Guid>(type: "TEXT", nullable: false),
                    EvaluationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProblemTitle = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    ProblemDescription = table.Column<string>(type: "TEXT", nullable: false),
                    FunctionSignature = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Category = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Difficulty = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    ModelName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    SampleType = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Verdict = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Score = table.Column<double>(type: "REAL", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FineTuneSamples", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FineTuneSamples_Category",
                table: "FineTuneSamples",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_FineTuneSamples_Difficulty",
                table: "FineTuneSamples",
                column: "Difficulty");

            migrationBuilder.CreateIndex(
                name: "IX_FineTuneSamples_EvaluationId",
                table: "FineTuneSamples",
                column: "EvaluationId");

            migrationBuilder.CreateIndex(
                name: "IX_FineTuneSamples_SampleType",
                table: "FineTuneSamples",
                column: "SampleType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FineTuneSamples");
        }
    }
}
