using Microsoft.EntityFrameworkCore;
using MRCD.Application.Abstracts;
using MRCD.Application.Abstracts.Security;
using MRCD.Domain.AccountingMovement;
using MRCD.Domain.Attendance;
using MRCD.Domain.Charge;
using MRCD.Domain.Degree;
using MRCD.Domain.Document;
using MRCD.Domain.Parent;
using MRCD.Domain.Person;
using MRCD.Domain.Planner;
using MRCD.Domain.Role;
using MRCD.Domain.Sacrament;
using MRCD.Domain.User;
using MRCD.Infrastructure.Security;

namespace MRCD.Infrastructure.Persistence;

internal sealed class AppContext(
    DbContextOptions<AppContext> options,
    IEncryptionService service
) : DbContext(options), IPersistenceContext
{
    private readonly IEncryptionService _service = service;

    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<Charge> Charges => Set<Charge>();
    public DbSet<Degree> Degrees => Set<Degree>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<Parent> Parents => Set<Parent>();
    public DbSet<ParentPerson> ParentsPersons => Set<ParentPerson>();
    public DbSet<Person> People => Set<Person>();
    public DbSet<PersonCharge> PersonCharges => Set<PersonCharge>();
    public DbSet<PersonDocument> PersonDocuments => Set<PersonDocument>();
    public DbSet<PersonSacrament> PersonSacraments => Set<PersonSacrament>();
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<ActivityStage> ActivitiesStages => Set<ActivityStage>();
    public DbSet<Stage> Stages => Set<Stage>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<RolePermission> RolesPermissions => Set<RolePermission>();
    public DbSet<Sacrament> Sacraments => Set<Sacrament>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<AccountingMovement> AccountingMovements => Set<AccountingMovement>();

    protected override void OnModelCreating(
        ModelBuilder builder
    )
    {
        builder.ApplyConfigurationsFromAssembly(typeof(AppContext).Assembly);
        builder.Entity<User>()
            .Property(u => u.Password)
            .HasColumnType("longtext")
            .HasConversion(new EncryptedRequiredStringConverter(_service));
        builder.Entity<Person>()
            .Property(e => e.Address)
            .HasColumnType("longtext")
            .HasConversion(new EncryptedStringConverter(_service));
        builder.Entity<Person>()
            .Property(e => e.Phone)
            .HasColumnType("longtext")
            .HasConversion(new EncryptedStringConverter(_service));
        builder.Entity<Parent>()
            .Property(e => e.Phone)
            .HasColumnType("longtext")
            .HasConversion(new EncryptedStringConverter(_service));
        base.OnModelCreating(builder);
    }

    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken
    ) => base.SaveChangesAsync(cancellationToken);
}