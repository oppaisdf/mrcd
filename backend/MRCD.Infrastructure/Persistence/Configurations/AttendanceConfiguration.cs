using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRCD.Domain.Attendance;
using MRCD.Domain.Person;
using MRCD.Domain.User;

namespace MRCD.Infrastructure.Persistence.Configurations;

internal sealed class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(
        EntityTypeBuilder<Attendance> builder
    )
    {
        builder.ToTable("attendance");
        builder
            .HasKey(e => e.ID)
            .HasName("PK_Attendance_ID");
        builder
            .Property(e => e.ID)
            .ValueGeneratedNever();
        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .HasConstraintName("FK_Attendance_UserId_User_ID");
        builder
            .HasOne<Person>()
            .WithMany()
            .HasForeignKey(e => e.PersonId)
            .HasConstraintName("FK_Attendance_PersonId_Person_ID");
    }
}