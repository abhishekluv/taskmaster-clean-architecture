using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TaskMaster.Domain.Entities;

namespace TaskMaster.Infrastructure.Persistence.Configurations
{
    internal class ReminderConfiguration : IEntityTypeConfiguration<Reminder>
    {
        public void Configure(EntityTypeBuilder<Reminder> builder)
        {
            builder.ToTable("Reminders");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.ReminderTime)
                .IsRequired();

            builder.Property(r => r.IsSent)
                .IsRequired();
        }
    }
}
