using Microsoft.EntityFrameworkCore;
using TaskMaster.Domain.Entities;

namespace TaskMaster.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Project> Projects { get; }
        DbSet<TaskItem> TaskItems { get; }
        DbSet<Tag> Tags { get; }
        DbSet<Reminder> Reminders { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
