using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Benchmark.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEvaluationResults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Evaluations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProblemId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ModelName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    OverallVerdict = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    PassedCount = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Score = table.Column<double>(type: "REAL", nullable: false),
                    GeneratedCode = table.Column<string>(type: "TEXT", nullable: true),
                    EvaluatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evaluations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EvaluationTestCaseResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Input = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    ExpectedOutput = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    ActualOutput = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Verdict = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    ExecutionTimeMs = table.Column<long>(type: "INTEGER", nullable: false),
                    ErrorMessage = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    EvaluationId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluationTestCaseResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvaluationTestCaseResults_Evaluations_EvaluationId",
                        column: x => x.EvaluationId,
                        principalTable: "Evaluations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_EvaluatedAt",
                table: "Evaluations",
                column: "EvaluatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_ModelName",
                table: "Evaluations",
                column: "ModelName");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_ProblemId",
                table: "Evaluations",
                column: "ProblemId");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationTestCaseResults_EvaluationId",
                table: "EvaluationTestCaseResults",
                column: "EvaluationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EvaluationTestCaseResults");

            migrationBuilder.DropTable(
                name: "Evaluations");
        }
    }
}
