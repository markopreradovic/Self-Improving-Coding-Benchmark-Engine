using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Benchmark.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddModelVersions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModelVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ModelName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    VersionNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    BaseModel = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    FilePath = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    AccuracyScore = table.Column<double>(type: "REAL", nullable: false),
                    TrainingSamples = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelVersions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModelVersions_IsActive",
                table: "ModelVersions",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ModelVersions_ModelName",
                table: "ModelVersions",
                column: "ModelName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModelVersions");
        }
    }
}
