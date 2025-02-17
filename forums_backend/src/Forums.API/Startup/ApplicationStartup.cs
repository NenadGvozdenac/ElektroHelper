
using System.Reflection;
using forums_backend.src.Forums.Application.BackgroundServices;
using forums_backend.src.Forums.Application.Features.Posts.SyncAllPosts;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;

namespace forums_backend.src.Forums.API.Startup;

public static class ApplicationStartup
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        SetupDatabases(services);
        SetupMediatR(services);
        SetupBackgroundServices(services);

        return services;
    }

    private static void SetupBackgroundServices(IServiceCollection services)
    {
        services.AddHostedService<PostSyncService>();
        services.AddScoped<PostSyncService>();
    }

    private static void SetupDatabases(IServiceCollection services)
    {
        services.AddScoped<IGraphDatabaseContext, Neo4jDatabaseContext>();
        services.AddScoped<IVectorDatabaseContext, VectorDatabaseContext>();
    }

    private static void SetupMediatR(IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}