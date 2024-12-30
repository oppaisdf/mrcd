using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "actionslog",
            columns: table => new
            {
                Id = table.Column<short>(type: "smallint", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Name = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ActionLog_Id", x => x.Id);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "aspnetroles",
            columns: table => new
            {
                Id = table.Column<string>(type: "varchar(255)", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                NormalizedName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_aspnetroles", x => x.Id);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "aspnetusers",
            columns: table => new
            {
                Id = table.Column<string>(type: "varchar(255)", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                UserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                NormalizedUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                NormalizedEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                PasswordHash = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                SecurityStamp = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                PhoneNumber = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                AccessFailedCount = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_aspnetusers", x => x.Id);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "degrees",
            columns: table => new
            {
                Id = table.Column<short>(type: "smallint", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Name = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Degree_Id", x => x.Id);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "sacraments",
            columns: table => new
            {
                Id = table.Column<short>(type: "smallint", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Name = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Sacrament_Id", x => x.Id);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "logs",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                UserId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Date = table.Column<DateTime>(type: "datetime(0)", nullable: false),
                ActionId = table.Column<short>(type: "smallint", nullable: false),
                Details = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Log_Id", x => x.Id);
                table.ForeignKey(
                    name: "FK_Log_ActionId",
                    column: x => x.ActionId,
                    principalTable: "actionslog",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "aspnetroleclaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                RoleId = table.Column<string>(type: "varchar(255)", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                ClaimType = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_aspnetroleclaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_aspnetroleclaims_aspnetroles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "aspnetroles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "aspnetuserclaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                ClaimType = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_aspnetuserclaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_aspnetuserclaims_aspnetusers_UserId",
                    column: x => x.UserId,
                    principalTable: "aspnetusers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "aspnetuserlogins",
            columns: table => new
            {
                LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                ProviderKey = table.Column<string>(type: "varchar(255)", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                ProviderDisplayName = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_aspnetuserlogins", x => new { x.LoginProvider, x.ProviderKey });
                table.ForeignKey(
                    name: "FK_aspnetuserlogins_aspnetusers_UserId",
                    column: x => x.UserId,
                    principalTable: "aspnetusers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "aspnetuserroles",
            columns: table => new
            {
                UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                RoleId = table.Column<string>(type: "varchar(255)", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_aspnetuserroles", x => new { x.UserId, x.RoleId });
                table.ForeignKey(
                    name: "FK_aspnetuserroles_aspnetroles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "aspnetroles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_aspnetuserroles_aspnetusers_UserId",
                    column: x => x.UserId,
                    principalTable: "aspnetusers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "aspnetusertokens",
            columns: table => new
            {
                UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Name = table.Column<string>(type: "varchar(255)", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Value = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_aspnetusertokens", x => new { x.UserId, x.LoginProvider, x.Name });
                table.ForeignKey(
                    name: "FK_aspnetusertokens_aspnetusers_UserId",
                    column: x => x.UserId,
                    principalTable: "aspnetusers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "people",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Hash = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Name = table.Column<byte[]>(type: "varbinary(128)", maxLength: 50, nullable: false),
                IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                Gender = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                Day = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                DOB = table.Column<DateTime>(type: "datetime(0)", nullable: false),
                Parish = table.Column<byte[]>(type: "varbinary(80)", maxLength: 30, nullable: true),
                DegreeId = table.Column<short>(type: "smallint", nullable: false),
                Address = table.Column<byte[]>(type: "varbinary(224)", maxLength: 100, nullable: true),
                Phone = table.Column<byte[]>(type: "varbinary(32)", maxLength: 8, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Person_Id", x => x.Id);
                table.ForeignKey(
                    name: "FK_Person_DegreeId",
                    column: x => x.DegreeId,
                    principalTable: "degrees",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "attendance",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                UserId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                PersonId = table.Column<int>(type: "int", nullable: false),
                Date = table.Column<DateTime>(type: "datetime(0)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Attendance_Id", x => x.Id);
                table.ForeignKey(
                    name: "FK_Attendance_PersonId",
                    column: x => x.PersonId,
                    principalTable: "people",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "godparents",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                PersonId = table.Column<int>(type: "int", nullable: false),
                Name = table.Column<byte[]>(type: "varbinary(128)", maxLength: 50, nullable: false),
                Gender = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Godparent_Id", x => x.Id);
                table.ForeignKey(
                    name: "FK_Godparent_PersonId",
                    column: x => x.PersonId,
                    principalTable: "people",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "parents",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                PersonId = table.Column<int>(type: "int", nullable: false),
                Name = table.Column<byte[]>(type: "varbinary(128)", maxLength: 50, nullable: false),
                Gender = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Parent_Id", x => x.Id);
                table.ForeignKey(
                    name: "FK_Parent_PersonId",
                    column: x => x.PersonId,
                    principalTable: "people",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "peoplesacraments",
            columns: table => new
            {
                SacramentId = table.Column<short>(type: "smallint", nullable: false),
                PersonId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PersonSacrament_PersonId_SacramentId", x => new { x.PersonId, x.SacramentId });
                table.ForeignKey(
                    name: "FK_PersonSacrament_PersonId",
                    column: x => x.PersonId,
                    principalTable: "people",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_PersonSacrament_SacramentId",
                    column: x => x.SacramentId,
                    principalTable: "sacraments",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateIndex(
            name: "IX_aspnetroleclaims_RoleId",
            table: "aspnetroleclaims",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "RoleNameIndex",
            table: "aspnetroles",
            column: "NormalizedName",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_aspnetuserclaims_UserId",
            table: "aspnetuserclaims",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_aspnetuserlogins_UserId",
            table: "aspnetuserlogins",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_aspnetuserroles_RoleId",
            table: "aspnetuserroles",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "EmailIndex",
            table: "aspnetusers",
            column: "NormalizedEmail");

        migrationBuilder.CreateIndex(
            name: "UserNameIndex",
            table: "aspnetusers",
            column: "NormalizedUserName",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_attendance_PersonId",
            table: "attendance",
            column: "PersonId");

        migrationBuilder.CreateIndex(
            name: "IX_godparents_PersonId",
            table: "godparents",
            column: "PersonId");

        migrationBuilder.CreateIndex(
            name: "IX_logs_ActionId",
            table: "logs",
            column: "ActionId");

        migrationBuilder.CreateIndex(
            name: "IX_parents_PersonId",
            table: "parents",
            column: "PersonId");

        migrationBuilder.CreateIndex(
            name: "IX_people_DegreeId",
            table: "people",
            column: "DegreeId");

        migrationBuilder.CreateIndex(
            name: "IX_peoplesacraments_SacramentId",
            table: "peoplesacraments",
            column: "SacramentId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "aspnetroleclaims");

        migrationBuilder.DropTable(
            name: "aspnetuserclaims");

        migrationBuilder.DropTable(
            name: "aspnetuserlogins");

        migrationBuilder.DropTable(
            name: "aspnetuserroles");

        migrationBuilder.DropTable(
            name: "aspnetusertokens");

        migrationBuilder.DropTable(
            name: "attendance");

        migrationBuilder.DropTable(
            name: "godparents");

        migrationBuilder.DropTable(
            name: "logs");

        migrationBuilder.DropTable(
            name: "parents");

        migrationBuilder.DropTable(
            name: "peoplesacraments");

        migrationBuilder.DropTable(
            name: "aspnetroles");

        migrationBuilder.DropTable(
            name: "aspnetusers");

        migrationBuilder.DropTable(
            name: "actionslog");

        migrationBuilder.DropTable(
            name: "people");

        migrationBuilder.DropTable(
            name: "sacraments");

        migrationBuilder.DropTable(
            name: "degrees");
    }
}