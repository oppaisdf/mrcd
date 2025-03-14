using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations;

/// <inheritdoc />
public partial class AddDocuments : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<byte[]>(
            name: "Parish",
            table: "people",
            type: "varbinary(48)",
            maxLength: 30,
            nullable: true,
            oldClrType: typeof(byte[]),
            oldType: "varbinary(80)",
            oldMaxLength: 30,
            oldNullable: true);

        migrationBuilder.AlterColumn<byte[]>(
            name: "Name",
            table: "people",
            type: "varbinary(96)",
            maxLength: 65,
            nullable: false,
            oldClrType: typeof(byte[]),
            oldType: "varbinary(128)",
            oldMaxLength: 50);

        migrationBuilder.AlterColumn<byte[]>(
            name: "Address",
            table: "people",
            type: "varbinary(128)",
            maxLength: 100,
            nullable: true,
            oldClrType: typeof(byte[]),
            oldType: "varbinary(224)",
            oldMaxLength: 100,
            oldNullable: true);

        migrationBuilder.AlterColumn<byte[]>(
            name: "Name",
            table: "parents",
            type: "varbinary(80)",
            maxLength: 50,
            nullable: false,
            oldClrType: typeof(byte[]),
            oldType: "varbinary(128)",
            oldMaxLength: 50);

        migrationBuilder.CreateTable(
            name: "documents",
            columns: table => new
            {
                Id = table.Column<short>(type: "smallint", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Documents_Id", x => x.Id);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "peopledocuments",
            columns: table => new
            {
                PersonId = table.Column<int>(type: "int", nullable: false),
                DocumentId = table.Column<short>(type: "smallint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PersonDocument_PersonId_DocumentId", x => new { x.PersonId, x.DocumentId });
                table.ForeignKey(
                    name: "FK_PeopleDocuments_DocumentId",
                    column: x => x.DocumentId,
                    principalTable: "documents",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_PeopleDocuments_PersonId",
                    column: x => x.PersonId,
                    principalTable: "people",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateIndex(
            name: "IX_peopledocuments_DocumentId",
            table: "peopledocuments",
            column: "DocumentId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "peopledocuments");

        migrationBuilder.DropTable(
            name: "documents");

        migrationBuilder.AlterColumn<byte[]>(
            name: "Parish",
            table: "people",
            type: "varbinary(80)",
            maxLength: 30,
            nullable: true,
            oldClrType: typeof(byte[]),
            oldType: "varbinary(48)",
            oldMaxLength: 30,
            oldNullable: true);

        migrationBuilder.AlterColumn<byte[]>(
            name: "Name",
            table: "people",
            type: "varbinary(128)",
            maxLength: 50,
            nullable: false,
            oldClrType: typeof(byte[]),
            oldType: "varbinary(96)",
            oldMaxLength: 65);

        migrationBuilder.AlterColumn<byte[]>(
            name: "Address",
            table: "people",
            type: "varbinary(224)",
            maxLength: 100,
            nullable: true,
            oldClrType: typeof(byte[]),
            oldType: "varbinary(128)",
            oldMaxLength: 100,
            oldNullable: true);

        migrationBuilder.AlterColumn<byte[]>(
            name: "Name",
            table: "parents",
            type: "varbinary(128)",
            maxLength: 50,
            nullable: false,
            oldClrType: typeof(byte[]),
            oldType: "varbinary(80)",
            oldMaxLength: 50);
    }
}
