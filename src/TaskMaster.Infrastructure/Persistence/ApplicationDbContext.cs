using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskMaster.Application.Common.Interfaces;
using TaskMaster.Domain.Entities;
using TaskMaster.Infrastructure.Identity;

namespace TaskMaster.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
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
            //this is important for Identity bcz Identity adds its own tables
            base.OnModelCreating(modelBuilder);

            //Apply all IEntityTypeConfiguration<> from this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);          
        }
    }
}
