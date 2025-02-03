
using forums_backend.src.Forums.Internal.API.Public;
using forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;
using forums_backend.src.Forums.Internal.Core.UseCases;
using forums_backend.src.Forums.Internal.Infrastructure.Database.Repositories;

namespace forums_backend.src.Forums.Internal.Infrastructure;

public static class ApplicationStartup {
    public static IServiceCollection ConfigureApplication(this IServiceCollection services) {
        SetupCore(services);
        SetupInfrastructure(services);

        return services;
    }

    // This method is used to setup the core services of the application
    private static void SetupCore(IServiceCollection services) {
        services.AddScoped<IForumsService, ForumsService>();
    }
    
    // This method is used to setup the infrastructure services of the application
    private static void SetupInfrastructure(IServiceCollection services) {
        services.AddScoped<IForumsRepository, ForumsRepository>();
    }
}