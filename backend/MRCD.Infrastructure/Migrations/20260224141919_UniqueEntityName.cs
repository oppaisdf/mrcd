using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MRCD.Infrastructure.Migrations;

/// <inheritdoc />
public partial class UniqueEntityName : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            name: "UX_User_Username",
            table: "user",
            column: "Username",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "UX_Stage_Name",
            table: "stage",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "UX_Sacrament_Name",
            table: "sacrament",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "UX_Role_Name",
            table: "role",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "UX_Person_NormalizedName",
            table: "person",
            column: "NormalizedName",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "UX_Permission_Name",
            table: "permission",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "UX_Parent_NormalizedName",
            table: "parent",
            column: "NormalizedName",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "UX_Document_Name",
            table: "document",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "UX_Degree_Name",
            table: "degree",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "UX_Charge_Name",
            table: "charge",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "UX_Activity_Name",
            table: "activity",
            column: "Name",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "UX_User_Username",
            table: "user");

        migrationBuilder.DropIndex(
            name: "UX_Stage_Name",
            table: "stage");

        migrationBuilder.DropIndex(
            name: "UX_Sacrament_Name",
            table: "sacrament");

        migrationBuilder.DropIndex(
            name: "UX_Role_Name",
            table: "role");

        migrationBuilder.DropIndex(
            name: "UX_Person_NormalizedName",
            table: "person");

        migrationBuilder.DropIndex(
            name: "UX_Permission_Name",
            table: "permission");

        migrationBuilder.DropIndex(
            name: "UX_Parent_NormalizedName",
            table: "parent");

        migrationBuilder.DropIndex(
            name: "UX_Document_Name",
            table: "document");

        migrationBuilder.DropIndex(
            name: "UX_Degree_Name",
            table: "degree");

        migrationBuilder.DropIndex(
            name: "UX_Charge_Name",
            table: "charge");

        migrationBuilder.DropIndex(
            name: "UX_Activity_Name",
            table: "activity");
    }
}