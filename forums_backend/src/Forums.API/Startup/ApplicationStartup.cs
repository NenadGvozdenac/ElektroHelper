
using System.Reflection;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;

namespace forums_backend.src.Forums.API.Startup;

public static class ApplicationStartup
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        SetupDatabase(services);
        SetupMediatR(services);

        return services;
    }

    private static void SetupDatabase(IServiceCollection services)
    {
        services.AddScoped<IGraphDatabaseContext, Neo4jDatabaseContext>();
    }

    private static void SetupMediatR(IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}