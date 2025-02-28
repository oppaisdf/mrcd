﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using api.Data;

#nullable disable

namespace api.Migrations;

[DbContext(typeof(MerContext))]
partial class MerContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "8.0.8")
            .HasAnnotation("Relational:MaxIdentifierLength", 64);

        MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
            {
                b.Property<string>("Id")
                    .HasColumnType("varchar(255)");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken()
                    .HasColumnType("longtext");

                b.Property<string>("Name")
                    .HasMaxLength(256)
                    .HasColumnType("varchar(256)");

                b.Property<string>("NormalizedName")
                    .HasMaxLength(256)
                    .HasColumnType("varchar(256)");

                b.HasKey("Id");

                b.HasIndex("NormalizedName")
                    .IsUnique()
                    .HasDatabaseName("RoleNameIndex");

                b.ToTable("aspnetroles", (string)null);
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("ClaimType")
                    .HasColumnType("longtext");

                b.Property<string>("ClaimValue")
                    .HasColumnType("longtext");

                b.Property<string>("RoleId")
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                b.HasKey("Id");

                b.HasIndex("RoleId");

                b.ToTable("aspnetroleclaims", (string)null);
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
            {
                b.Property<string>("Id")
                    .HasColumnType("varchar(255)");

                b.Property<int>("AccessFailedCount")
                    .HasColumnType("int");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken()
                    .HasColumnType("longtext");

                b.Property<string>("Email")
                    .HasMaxLength(256)
                    .HasColumnType("varchar(256)");

                b.Property<bool>("EmailConfirmed")
                    .HasColumnType("tinyint(1)");

                b.Property<bool>("LockoutEnabled")
                    .HasColumnType("tinyint(1)");

                b.Property<DateTimeOffset?>("LockoutEnd")
                    .HasColumnType("datetime(6)");

                b.Property<string>("NormalizedEmail")
                    .HasMaxLength(256)
                    .HasColumnType("varchar(256)");

                b.Property<string>("NormalizedUserName")
                    .HasMaxLength(256)
                    .HasColumnType("varchar(256)");

                b.Property<string>("PasswordHash")
                    .HasColumnType("longtext");

                b.Property<string>("PhoneNumber")
                    .HasColumnType("longtext");

                b.Property<bool>("PhoneNumberConfirmed")
                    .HasColumnType("tinyint(1)");

                b.Property<string>("SecurityStamp")
                    .HasColumnType("longtext");

                b.Property<bool>("TwoFactorEnabled")
                    .HasColumnType("tinyint(1)");

                b.Property<string>("UserName")
                    .HasMaxLength(256)
                    .HasColumnType("varchar(256)");

                b.HasKey("Id");

                b.HasIndex("NormalizedEmail")
                    .HasDatabaseName("EmailIndex");

                b.HasIndex("NormalizedUserName")
                    .IsUnique()
                    .HasDatabaseName("UserNameIndex");

                b.ToTable("aspnetusers", (string)null);
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("ClaimType")
                    .HasColumnType("longtext");

                b.Property<string>("ClaimValue")
                    .HasColumnType("longtext");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.ToTable("aspnetuserclaims", (string)null);
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
            {
                b.Property<string>("LoginProvider")
                    .HasColumnType("varchar(255)");

                b.Property<string>("ProviderKey")
                    .HasColumnType("varchar(255)");

                b.Property<string>("ProviderDisplayName")
                    .HasColumnType("longtext");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                b.HasKey("LoginProvider", "ProviderKey");

                b.HasIndex("UserId");

                b.ToTable("aspnetuserlogins", (string)null);
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
            {
                b.Property<string>("UserId")
                    .HasColumnType("varchar(255)");

                b.Property<string>("RoleId")
                    .HasColumnType("varchar(255)");

                b.HasKey("UserId", "RoleId");

                b.HasIndex("RoleId");

                b.ToTable("aspnetuserroles", (string)null);
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
            {
                b.Property<string>("UserId")
                    .HasColumnType("varchar(255)");

                b.Property<string>("LoginProvider")
                    .HasColumnType("varchar(255)");

                b.Property<string>("Name")
                    .HasColumnType("varchar(255)");

                b.Property<string>("Value")
                    .HasColumnType("longtext");

                b.HasKey("UserId", "LoginProvider", "Name");

                b.ToTable("aspnetusertokens", (string)null);
            });

        modelBuilder.Entity("api.Models.Entities.ActionLog", b =>
            {
                b.Property<short?>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("smallint");

                MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<short?>("Id"));

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnType("varchar(10)");

                b.HasKey("Id")
                    .HasName("PK_ActionLog_Id");

                b.ToTable("actionslog");
            });

        modelBuilder.Entity("api.Models.Entities.Attendance", b =>
            {
                b.Property<int?>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int?>("Id"));

                b.Property<DateTime>("Date")
                    .HasColumnType("datetime(0)");

                b.Property<bool>("IsAttendance")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValue(true);

                b.Property<int>("PersonId")
                    .HasColumnType("int");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnType("varchar(255)");

                b.HasKey("Id")
                    .HasName("PK_Attendance_Id");

                b.HasIndex("PersonId");

                b.ToTable("attendance");
            });

        modelBuilder.Entity("api.Models.Entities.Charge", b =>
            {
                b.Property<short?>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("smallint");

                MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<short?>("Id"));

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(11)
                    .HasColumnType("varchar(11)");

                b.Property<decimal>("Total")
                    .HasPrecision(6, 2)
                    .HasColumnType("decimal(6,2)");

                b.HasKey("Id")
                    .HasName("PK_Charge_Id");

                b.ToTable("charges");
            });

        modelBuilder.Entity("api.Models.Entities.Degree", b =>
            {
                b.Property<short?>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("smallint");

                MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<short?>("Id"));

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnType("varchar(10)");

                b.HasKey("Id")
                    .HasName("PK_Degree_Id");

                b.ToTable("degrees");
            });

        modelBuilder.Entity("api.Models.Entities.Log", b =>
            {
                b.Property<int?>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int?>("Id"));

                b.Property<short>("ActionId")
                    .HasColumnType("smallint");

                b.Property<DateTime>("Date")
                    .HasColumnType("datetime(0)");

                b.Property<string>("Details")
                    .HasMaxLength(50)
                    .HasColumnType("varchar(50)");

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnType("varchar(255)");

                b.HasKey("Id")
                    .HasName("PK_Log_Id");

                b.HasIndex("ActionId");

                b.ToTable("logs");
            });

        modelBuilder.Entity("api.Models.Entities.Parent", b =>
            {
                b.Property<int?>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int?>("Id"));

                b.Property<bool>("Gender")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValue(true);

                b.Property<byte[]>("Name")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("varbinary(128)");

                b.Property<string>("NameHash")
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                b.Property<byte[]>("Phone")
                    .HasColumnType("varbinary(32)");

                b.HasKey("Id")
                    .HasName("PK_Parent_Id");

                b.HasIndex("NameHash")
                    .IsUnique()
                    .HasDatabaseName("UX_Parents_NameHash");

                b.ToTable("parents");
            });

        modelBuilder.Entity("api.Models.Entities.ParentPerson", b =>
            {
                b.Property<int>("ParentId")
                    .HasColumnType("int");

                b.Property<int>("PersonId")
                    .HasColumnType("int");

                b.Property<bool>("IsParent")
                    .HasColumnType("tinyint(1)");

                b.HasKey("ParentId", "PersonId", "IsParent")
                    .HasName("PK_ParentPerson_ParentId_PersonId_IsParent");

                b.HasIndex("PersonId");

                b.ToTable("parentspeople");
            });

        modelBuilder.Entity("api.Models.Entities.Person", b =>
            {
                b.Property<int?>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int?>("Id"));

                b.Property<byte[]>("Address")
                    .HasMaxLength(100)
                    .HasColumnType("varbinary(224)");

                b.Property<DateTime>("DOB")
                    .HasColumnType("datetime(0)");

                b.Property<bool>("Day")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValue(false);

                b.Property<short>("DegreeId")
                    .HasColumnType("smallint");

                b.Property<bool>("Gender")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValue(true);

                b.Property<string>("Hash")
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnType("varchar(64)");

                b.Property<bool>("IsActive")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValue(true);

                b.Property<byte[]>("Name")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("varbinary(128)");

                b.Property<byte[]>("Parish")
                    .HasMaxLength(30)
                    .HasColumnType("varbinary(80)");

                b.Property<byte[]>("Phone")
                    .HasMaxLength(8)
                    .HasColumnType("varbinary(32)");

                b.HasKey("Id")
                    .HasName("PK_Person_Id");

                b.HasIndex("DegreeId");

                b.ToTable("people");
            });

        modelBuilder.Entity("api.Models.Entities.PersonCharge", b =>
            {
                b.Property<int>("PersonId")
                    .HasColumnType("int");

                b.Property<short>("ChargeId")
                    .HasColumnType("smallint");

                b.Property<decimal>("Total")
                    .HasPrecision(6, 2)
                    .HasColumnType("decimal(6,2)");

                b.HasKey("PersonId", "ChargeId")
                    .HasName("PK_PersonCharge_PersonId_ChargeId");

                b.HasIndex("ChargeId");

                b.ToTable("peoplecharges");
            });

        modelBuilder.Entity("api.Models.Entities.PersonSacrament", b =>
            {
                b.Property<int>("PersonId")
                    .HasColumnType("int");

                b.Property<short>("SacramentId")
                    .HasColumnType("smallint");

                b.HasKey("PersonId", "SacramentId")
                    .HasName("PK_PersonSacrament_PersonId_SacramentId");

                b.HasIndex("SacramentId");

                b.ToTable("peoplesacraments");
            });

        modelBuilder.Entity("api.Models.Entities.Sacrament", b =>
            {
                b.Property<short?>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("smallint");

                MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<short?>("Id"));

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnType("varchar(10)");

                b.HasKey("Id")
                    .HasName("PK_Sacrament_Id");

                b.ToTable("sacraments");
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        modelBuilder.Entity("api.Models.Entities.Attendance", b =>
            {
                b.HasOne("api.Models.Entities.Person", null)
                    .WithMany()
                    .HasForeignKey("PersonId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired()
                    .HasConstraintName("FK_Attendance_PersonId");
            });

        modelBuilder.Entity("api.Models.Entities.Log", b =>
            {
                b.HasOne("api.Models.Entities.ActionLog", null)
                    .WithMany()
                    .HasForeignKey("ActionId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired()
                    .HasConstraintName("FK_Log_ActionId");
            });

        modelBuilder.Entity("api.Models.Entities.ParentPerson", b =>
            {
                b.HasOne("api.Models.Entities.Parent", null)
                    .WithMany()
                    .HasForeignKey("ParentId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired()
                    .HasConstraintName("FK_PersonParent_ParentId");

                b.HasOne("api.Models.Entities.Person", null)
                    .WithMany()
                    .HasForeignKey("PersonId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired()
                    .HasConstraintName("FK_PersonParent_PersonId");
            });

        modelBuilder.Entity("api.Models.Entities.Person", b =>
            {
                b.HasOne("api.Models.Entities.Degree", null)
                    .WithMany()
                    .HasForeignKey("DegreeId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired()
                    .HasConstraintName("FK_Person_DegreeId");
            });

        modelBuilder.Entity("api.Models.Entities.PersonCharge", b =>
            {
                b.HasOne("api.Models.Entities.Charge", null)
                    .WithMany()
                    .HasForeignKey("ChargeId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired()
                    .HasConstraintName("FK_PersonCharge_ChargeId");

                b.HasOne("api.Models.Entities.Person", null)
                    .WithMany()
                    .HasForeignKey("PersonId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired()
                    .HasConstraintName("FK_PersonCharge_PersonId");
            });

        modelBuilder.Entity("api.Models.Entities.PersonSacrament", b =>
            {
                b.HasOne("api.Models.Entities.Person", null)
                    .WithMany()
                    .HasForeignKey("PersonId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired()
                    .HasConstraintName("FK_PersonSacrament_PersonId");

                b.HasOne("api.Models.Entities.Sacrament", null)
                    .WithMany()
                    .HasForeignKey("SacramentId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired()
                    .HasConstraintName("FK_PersonSacrament_SacramentId");
            });
#pragma warning restore 612, 618
    }
}
