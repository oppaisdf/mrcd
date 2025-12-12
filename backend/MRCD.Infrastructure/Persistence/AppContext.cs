using Microsoft.EntityFrameworkCore;
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

namespace MRCD.Infrastructure.Persistence;

internal sealed class AppContext(
    DbContextOptions<AppContext> options
) : DbContext(options)
{
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

    protected override void OnModelCreating(
        ModelBuilder builder
    ) => builder.ApplyConfigurationsFromAssembly(typeof(AppContext).Assembly);
}