using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TaskMaster.Domain.Entities;

namespace TaskMaster.Infrastructure.Persistence.Configurations
{
    internal class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.ToTable("Tasks");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Description)
                .HasMaxLength(2000);

            builder.Property(t => t.AssignedToUserId)
                .HasMaxLength(450);

            builder.Property(t => t.Status)
                .IsRequired();

            builder.Property(t => t.Priority)
                .IsRequired();

            builder.HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId);

            builder.HasMany(t => t.Tags)
                .WithMany(tag => tag.Tasks)
                .UsingEntity(j => j.ToTable("TaskTags"));

            builder.HasMany(t => t.Reminders)
                .WithOne(r => r.Task)
                .HasForeignKey(r => r.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
