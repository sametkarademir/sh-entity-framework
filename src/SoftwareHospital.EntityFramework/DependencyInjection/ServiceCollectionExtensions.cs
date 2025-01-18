using Microsoft.Extensions.DependencyInjection;
using SoftwareHospital.EntityFramework.Contexts;
using SoftwareHospital.EntityFramework.Models;

namespace SoftwareHospital.EntityFramework.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEntityFrameworkServices<TContext>(this IServiceCollection services, Action<EntityFrameworkSettings> configureEntityFrameworkSettings) where TContext : BaseDbContext
    {
        var entityFrameworkSettings = new EntityFrameworkSettings();
        configureEntityFrameworkSettings(entityFrameworkSettings);
        services.Configure(configureEntityFrameworkSettings);
        
        services.AddDbContext<TContext>();
        return services;
    }
}