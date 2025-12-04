using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TaskMaster.Application.Common.Interfaces;
using TaskMaster.Infrastructure.Identity;
using TaskMaster.Infrastructure.Persistence;

namespace TaskMaster.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("TaskMaster") ?? throw new InvalidOperationException("Connection string 'TaskMaster' not found!");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

            //Identity
            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
            }).AddRoles<IdentityRole>()
              .AddEntityFrameworkStores<ApplicationDbContext>();

            return services;
        }
    }
}
