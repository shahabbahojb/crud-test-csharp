using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                    // .ConfigureWarnings(builder =>
                    //     builder.Throw(RelationalEventId.MultipleCollectionIncludeWarning)
                    // );
                if (configuration["ComponentConfig:Environment"].Equals("Development")) {
                    // options.EnableSensitiveDataLogging();
                }
            });
            
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            return services;
        }
    }
}