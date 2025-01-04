using api.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Context;

public class MerContext(
    DbContextOptions<MerContext> options
) : IdentityDbContext<IdentityUser>(options)
{
    public required DbSet<Person> People { get; set; }
    public required DbSet<Parent> Parents { get; set; }
    public required DbSet<Godparent> Godparents { get; set; }
    public required DbSet<Sacrament> Sacraments { get; set; }
    public required DbSet<Degree> Degrees { get; set; }
    public required DbSet<ActionLog> ActionsLog { get; set; }
    public required DbSet<PersonSacrament> PeopleSacraments { get; set; }
    public required DbSet<Log> Logs { get; set; }
    public required DbSet<Attendance> Attendance { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        var encrypter = new Services.EncryptionConverter();

        // Nombre de entidadews a minúsculas, porque MySQL en Linux así trabaja: SHOW VARIABLES LIKE 'lower_case_table_names';
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName();
            if (tableName == null) continue;
            entity.SetTableName(tableName.ToLower());
        }

        builder.Entity<Person>(person =>
        {
            person
                .HasKey(p => p.Id)
                .HasName("PK_Person_Id");
            person
                .Property(p => p.DOB)
                .HasColumnType("datetime(0)");
            person
                .Property(p => p.Name)
                .HasConversion(encrypter!)
                .HasColumnType("varbinary(128)");
            person
                .Property(p => p.Gender)
                .HasDefaultValue(true);
            person
                .Property(p => p.Day)
                .HasDefaultValue(false);
            person
                .Property(p => p.Parish)
                .HasConversion(encrypter)
                .HasColumnType("varbinary(80)");
            person
                .HasOne<Degree>()
                .WithMany()
                .HasForeignKey(d => d.DegreeId)
                .HasConstraintName("FK_Person_DegreeId")
                .IsRequired();
            person
                .Property(p => p.Address)
                .HasConversion(encrypter!)
                .HasColumnType("varbinary(224)");
            person
                .Property(p => p.IsActive)
                .HasDefaultValue(true);
            person
                .Property(p => p.Phone)
                .HasConversion(encrypter)
                .HasColumnType("varbinary(32)");
        });

        builder.Entity<Parent>(parent =>
        {
            parent
                .HasKey(p => p.Id)
                .HasName("PK_Parent_Id");
            parent
                .Property(p => p.Name)
                .HasConversion(encrypter!)
                .HasColumnType("varbinary(128)");
            parent
                .Property(p => p.Phone)
                .HasConversion(encrypter)
                .HasColumnType("varbinary(32)");
            parent
                .HasOne<Person>()
                .WithMany()
                .HasForeignKey(p => p.PersonId)
                .HasConstraintName("FK_Parent_PersonId")
                .IsRequired();
            parent
                .Property(p => p.Gender)
                .HasDefaultValue(true);
        });

        builder.Entity<Godparent>(parent =>
        {
            parent
                .HasKey(p => p.Id)
                .HasName("PK_Godparent_Id");
            parent
                .Property(p => p.Name)
                .HasConversion(encrypter!)
                .HasColumnType("varbinary(128)");
            parent
                .HasOne<Person>()
                .WithMany()
                .HasForeignKey(p => p.PersonId)
                .HasConstraintName("FK_Godparent_PersonId")
                .IsRequired();
            parent
                .Property(p => p.Gender)
                .HasDefaultValue(true);
        });

        builder.Entity<Sacrament>(sacrament =>
        {
            sacrament
                .HasKey(s => s.Id)
                .HasName("PK_Sacrament_Id");
        });

        builder.Entity<PersonSacrament>(personSacrament =>
        {
            personSacrament
                .HasKey(ps => new { ps.PersonId, ps.SacramentId })
                .HasName("PK_PersonSacrament_PersonId_SacramentId");
            personSacrament
                .HasOne<Person>()
                .WithMany()
                .HasForeignKey(ps => ps.PersonId)
                .HasConstraintName("FK_PersonSacrament_PersonId")
                .IsRequired();
            personSacrament
                .HasOne<Sacrament>()
                .WithMany()
                .HasForeignKey(ps => ps.SacramentId)
                .HasConstraintName("FK_PersonSacrament_SacramentId")
                .IsRequired();
        });

        builder.Entity<Degree>(degree =>
        {
            degree
                .HasKey(d => d.Id)
                .HasName("PK_Degree_Id");
        });

        builder.Entity<Log>(log =>
        {
            log
                .HasKey(l => l.Id)
                .HasName("PK_Log_Id");
            log
                .Property(l => l.Date)
                .HasColumnType("datetime(0)");
            log
                .HasOne<ActionLog>()
                .WithMany()
                .HasForeignKey(l => l.ActionId)
                .HasConstraintName("FK_Log_ActionId")
                .IsRequired();
        });

        builder.Entity<ActionLog>(action =>
        {
            action
                .HasKey(a => a.Id)
                .HasName("PK_ActionLog_Id");
        });

        builder.Entity<Attendance>(attendance =>
        {
            attendance
                .HasKey(a => a.Id)
                .HasName("PK_Attendance_Id");
            attendance
                .Property(a => a.Date)
                .HasColumnType("datetime(0)");
            attendance
                .HasOne<Person>()
                .WithMany()
                .HasForeignKey(a => a.PersonId)
                .HasConstraintName("FK_Attendance_PersonId")
                .IsRequired();
        });
    }
}