using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations;

/// <inheritdoc />
public partial class AddPlanner : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "activities",
            columns: table => new
            {
                Id = table.Column<uint>(type: "int unsigned", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Date = table.Column<DateTime>(type: "datetime(6)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Activity_Id", x => x.Id);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "activitystages",
            columns: table => new
            {
                Id = table.Column<ushort>(type: "smallint unsigned", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ActivityStage_Id", x => x.Id);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "stagesofactivities",
            columns: table => new
            {
                ActivityId = table.Column<uint>(type: "int unsigned", nullable: false),
                StageId = table.Column<ushort>(type: "smallint unsigned", nullable: false),
                UserId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                MainUser = table.Column<bool>(type: "tinyint(1)", nullable: false),
                Notes = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_StagesOfActivities_ActivityId_StageId", x => new { x.ActivityId, x.StageId });
                table.ForeignKey(
                    name: "FK_StagesOfActivities_ActivityId",
                    column: x => x.ActivityId,
                    principalTable: "activities",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_StagesOfActivities_StageId",
                    column: x => x.StageId,
                    principalTable: "activitystages",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateIndex(
            name: "IX_stagesofactivities_StageId",
            table: "stagesofactivities",
            column: "StageId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "stagesofactivities");

        migrationBuilder.DropTable(
            name: "activities");

        migrationBuilder.DropTable(
            name: "activitystages");
    }
}
