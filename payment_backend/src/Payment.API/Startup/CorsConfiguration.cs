using Microsoft.Net.Http.Headers;

namespace payment_backend.src.Payment.API.Startup;

public static class CorsConfiguration
{
    public static IServiceCollection ConfigureCors(this IServiceCollection services, string corsPolicy)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: corsPolicy,
                builder =>
                {
                    builder.WithOrigins(ParseCorsOrigins())
                        .WithHeaders(HeaderNames.ContentType, HeaderNames.Authorization, "access_token")
                        .WithMethods("GET", "PUT", "POST", "PATCH", "DELETE", "OPTIONS");
                });
        });
        return services;
    }

    private static string[] ParseCorsOrigins()
    {
        var corsOrigins = new[] { "http://localhost:5173" };
        var corsOriginsPath = Environment.GetEnvironmentVariable("EXPLORER_CORS_ORIGINS");
        if (File.Exists(corsOriginsPath))
        {
            corsOrigins = File.ReadAllLines(corsOriginsPath);
        }

        return corsOrigins;
    }
}