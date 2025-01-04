using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations;
/// <inheritdoc />
public partial class AddParentPhone : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<byte[]>(
            name: "Phone",
            table: "parents",
            type: "varbinary(32)",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Phone",
            table: "parents");
    }
}
