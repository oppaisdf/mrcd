using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRCD.Domain.Planner;
using MRCD.Domain.User;

namespace MRCD.Infrastructure.Persistence.Configurations;

internal sealed class ActivityStageConfiguration : IEntityTypeConfiguration<ActivityStage>
{
    public void Configure(
        EntityTypeBuilder<ActivityStage> builder
    )
    {
        builder.ToTable("activity_stage");
        builder
            .HasKey(e => new { e.ActivityId, e.StageId, e.UserId })
            .HasName("PK_ActivityStage_ActivityId_StageId_UserId");
        builder
            .HasOne<Activity>()
            .WithMany()
            .HasForeignKey(e => e.ActivityId)
            .HasConstraintName("FK_ActivityStage_ActivityId_Activity_ID");
        builder
            .HasOne<Stage>()
            .WithMany()
            .HasForeignKey(e => e.StageId)
            .HasConstraintName("FK_ActivityStage_StageId_Stage_ID");
        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .HasConstraintName("FK_ActivityStage_UserId_User_ID");
    }
}