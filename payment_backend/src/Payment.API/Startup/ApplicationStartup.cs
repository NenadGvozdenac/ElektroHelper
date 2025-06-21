
using System.Reflection;

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
        // TODO: Implement database setup logic
    }

    private static void SetupMediatR(IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}