using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations;
/// <inheritdoc />
public partial class FixFKPersonsPeople : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_PersonParent_PersonId",
            table: "parentspeople");

        migrationBuilder.CreateIndex(
            name: "IX_parentspeople_PersonId",
            table: "parentspeople",
            column: "PersonId");

        migrationBuilder.AddForeignKey(
            name: "FK_PersonParent_PersonId",
            table: "parentspeople",
            column: "PersonId",
            principalTable: "people",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_PersonParent_PersonId",
            table: "parentspeople");

        migrationBuilder.DropIndex(
            name: "IX_parentspeople_PersonId",
            table: "parentspeople");

        migrationBuilder.AddForeignKey(
            name: "FK_PersonParent_PersonId",
            table: "parentspeople",
            column: "ParentId",
            principalTable: "people",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
