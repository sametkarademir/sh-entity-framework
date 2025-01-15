using Microsoft.Extensions.DependencyInjection;
using SoftwareHospital.EntityFramework.Contexts;

namespace SoftwareHospital.EntityFramework.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEntityFrameworkServices<TContext>(this IServiceCollection services) where TContext : BaseDbContext
    {
        services.AddDbContext<TContext>();
        return services;
    }
}