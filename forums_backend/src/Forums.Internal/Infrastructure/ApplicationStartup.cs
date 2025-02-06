
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using forums_backend.src.Forums.Internal.API.Public;
using forums_backend.src.Forums.Internal.Core.Domain.RepositoryInterfaces;
using forums_backend.src.Forums.Internal.Core.UseCases;
using forums_backend.src.Forums.Internal.Infrastructure.Database;
using forums_backend.src.Forums.Internal.Infrastructure.Database.Repositories;
using forums_backend.src.Forums.Internal.Infrastructure.Mappings;

namespace forums_backend.src.Forums.Internal.Infrastructure;

public static class ApplicationStartup
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        SetupDatabase(services);
        SetupAutomapper(services);
        SetupCore(services);
        SetupInfrastructure(services);

        return services;
    }

    private static void SetupAutomapper(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Neo4jMapperProfile));
        services.AddAutoMapper(typeof(AutoMapperProfile));
    }

    private static void SetupDatabase(IServiceCollection services)
    {
        services.AddScoped<IGraphDatabaseContext, Neo4jDatabaseContext>();
    }

    private static void SetupCore(IServiceCollection services)
    {
        services.AddScoped<IForumsService, ForumsService>();
        services.AddScoped<IPostsService, PostsService>();
        services.AddScoped<ICommentsService, CommentsService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUpvoteService, UpvoteService>();
        services.AddScoped<IDownvoteService, DownvoteService>();
    }

    private static void SetupInfrastructure(IServiceCollection services)
    {
        services.AddScoped<IForumsRepository, ForumsRepository>();
        services.AddScoped<IPostsRepository, PostsRepository>();
        services.AddScoped<ICommentsRepository, CommentsRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUpvotePostRepository, UpvotePostRepository>();
        services.AddScoped<IUpvoteCommentRepository, UpvoteCommentRepository>();
        services.AddScoped<IDownvotePostRepository, DownvotePostRepository>();
        services.AddScoped<IDownvoteCommentRepository, DownvoteCommentRepository>();
    }
}