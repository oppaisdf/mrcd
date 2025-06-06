using api.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public class MerContext(
    DbContextOptions<MerContext> options
) : IdentityDbContext<IdentityUser>(options)
{
    public required DbSet<Person> People { get; set; }
    public required DbSet<ParentPerson> ParentsPeople { get; set; }
    public required DbSet<Parent> Parents { get; set; }
    public required DbSet<Sacrament> Sacraments { get; set; }
    public required DbSet<Degree> Degrees { get; set; }
    public required DbSet<ActionLog> ActionsLog { get; set; }
    public required DbSet<PersonSacrament> PeopleSacraments { get; set; }
    public required DbSet<Log> Logs { get; set; }
    public required DbSet<Attendance> Attendance { get; set; }
    public required DbSet<Charge> Charges { get; set; }
    public required DbSet<PersonCharge> PeopleCharges { get; set; }
    public required DbSet<Document> Documents { get; set; }
    public required DbSet<PersonDocument> PeopleDocuments { get; set; }
    public required DbSet<Activity> Activities { get; set; }
    public required DbSet<ActivityStage> ActivityStages { get; set; }
    public required DbSet<StagesOfActivities> StagesOfActivities { get; set; }

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
                .HasColumnType("varbinary(96)");
            person
                .Property(p => p.Gender)
                .HasDefaultValue(true);
            person
                .Property(p => p.Day)
                .HasDefaultValue(false);
            person
                .Property(p => p.Parish)
                .HasConversion(encrypter)
                .HasColumnType("varbinary(48)");
            person
                .HasOne<Degree>()
                .WithMany()
                .HasForeignKey(d => d.DegreeId)
                .HasConstraintName("FK_Person_DegreeId")
                .IsRequired();
            person
                .Property(p => p.Address)
                .HasConversion(encrypter!)
                .HasColumnType("varbinary(128)");
            person
                .Property(p => p.IsActive)
                .HasDefaultValue(true);
            person
                .Property(p => p.Phone)
                .HasConversion(encrypter)
                .HasColumnType("varbinary(32)");
        });

        builder.Entity<ParentPerson>(pp =>
        {
            pp
                .HasKey(p => new { p.ParentId, p.PersonId, p.IsParent })
                .HasName("PK_ParentPerson_ParentId_PersonId_IsParent");
            pp
                .HasOne<Person>()
                .WithMany()
                .HasForeignKey(p => p.PersonId)
                .HasConstraintName("FK_PersonParent_PersonId")
                .IsRequired();
            pp
                .HasOne<Parent>()
                .WithMany()
                .HasForeignKey(p => p.ParentId)
                .HasConstraintName("FK_PersonParent_ParentId")
                .IsRequired();
        });

        builder.Entity<Parent>(parent =>
        {
            parent
                .HasKey(p => p.Id)
                .HasName("PK_Parent_Id");
            parent
                .Property(p => p.Name)
                .HasConversion(encrypter!)
                .HasColumnType("varbinary(80)");
            parent
                .Property(p => p.Phone)
                .HasConversion(encrypter)
                .HasColumnType("varbinary(32)");
            parent
                .Property(p => p.Gender)
                .HasDefaultValue(true);
            parent
                .HasIndex(c => c.NameHash)
                .IsUnique()
                .HasDatabaseName("UX_Parents_NameHash");
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
            attendance
                .Property(a => a.IsAttendance)
                .HasDefaultValue(true);
        });

        builder
            .Entity<Charge>()
            .HasKey(c => c.Id)
            .HasName("PK_Charge_Id");

        builder.Entity<PersonCharge>(charge =>
        {
            charge
                .HasKey(c => new { c.PersonId, c.ChargeId })
                .HasName("PK_PersonCharge_PersonId_ChargeId");
            charge
                .HasOne<Person>()
                .WithMany()
                .HasForeignKey(c => c.PersonId)
                .HasConstraintName("FK_PersonCharge_PersonId")
                .IsRequired();
            charge
                .HasOne<Charge>()
                .WithMany()
                .HasForeignKey(c => c.ChargeId)
                .HasConstraintName("FK_PersonCharge_ChargeId")
                .IsRequired();
        });

        builder.Entity<Document>()
            .HasKey(d => d.Id)
            .HasName("PK_Documents_Id");

        builder.Entity<PersonDocument>(docpe =>
        {
            docpe
                .HasKey(d => new { d.PersonId, d.DocumentId })
                .HasName("PK_PersonDocument_PersonId_DocumentId");
            docpe
                .HasOne<Person>()
                .WithMany()
                .HasForeignKey(p => p.PersonId)
                .HasConstraintName("FK_PeopleDocuments_PersonId")
                .IsRequired();
            docpe
                .HasOne<Document>()
                .WithMany()
                .HasForeignKey(d => d.DocumentId)
                .HasConstraintName("FK_PeopleDocuments_DocumentId")
                .IsRequired();
        });


        builder.Entity<Activity>()
            .HasKey(a => a.Id)
            .HasName("PK_Activity_Id");

        builder.Entity<ActivityStage>()
            .HasKey(acs => acs.Id)
            .HasName("PK_ActivityStage_Id");

        builder.Entity<StagesOfActivities>(stages =>
        {
            stages
                .HasKey(sa => new { sa.ActivityId, sa.StageId })
                .HasName("PK_StagesOfActivities_ActivityId_StageId");
            stages
                .HasOne<Activity>()
                .WithMany()
                .HasForeignKey(sa => sa.ActivityId)
                .HasConstraintName("FK_StagesOfActivities_ActivityId")
                .IsRequired();
            stages
                .HasOne<ActivityStage>()
                .WithMany()
                .HasForeignKey(sa => sa.StageId)
                .HasConstraintName("FK_StagesOfActivities_StageId")
                .IsRequired();
        });
    }
}