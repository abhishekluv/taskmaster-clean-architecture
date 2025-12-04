using Microsoft.EntityFrameworkCore;
using TaskMaster.Application.Common.Interfaces;
using TaskMaster.Domain.Entities;

namespace TaskMaster.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Project> Projects => Set<Project>();

        public DbSet<TaskItem> TaskItems => Set<TaskItem>();

        public DbSet<Tag> Tags => Set<Tag>();

        public DbSet<Reminder> Reminders => Set<Reminder>();

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Apply all IEntityTypeConfiguration<> from this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
