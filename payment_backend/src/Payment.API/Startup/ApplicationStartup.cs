
using System.Reflection;
using payment_backend.src.Payment.BuildingBlocks.Infrastructure.Database;

namespace payment_backend.src.Payment.API.Startup;

public static class ApplicationStartup
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        SetupDatabases(services);
        SetupMediatR(services);

        return services;
    }

    private static void SetupDatabases(IServiceCollection services)
    {
        services.AddScoped<IDocumentDatabaseContext, MongoDatabaseContext>();
    }

    private static void SetupMediatR(IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}