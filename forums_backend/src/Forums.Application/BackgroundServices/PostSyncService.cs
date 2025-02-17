using forums_backend.src.Forums.Application.Features.Posts.SyncAllPosts;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;

namespace forums_backend.src.Forums.Application.BackgroundServices;

public class PostSyncService : IPostSyncService
{
    private readonly ILogger<PostSyncService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private Timer? _timer;
    private const int SyncInterval = 5;

    public PostSyncService(ILogger<PostSyncService> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    private (IVectorDatabaseContext vectorDatabaseContext, IMediator mediator) GetServices()
    {
        var scope = _serviceScopeFactory.CreateScope();
        var vectorDatabaseContext = scope.ServiceProvider.GetRequiredService<IVectorDatabaseContext>();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        return (vectorDatabaseContext, mediator);
    }

    private async Task SyncPosts(List<PostDTO> posts, CancellationToken cancellationToken)
    {
        var (vectorDatabaseContext, mediator) = GetServices();

        if (posts.Count == 0)
        {
            _logger.LogInformation("[PostSyncService] No new posts to sync.");
            return;
        }

        var bulkIndexResponse = await vectorDatabaseContext.IndexData(posts, "forum_posts");

        if (!bulkIndexResponse.IsValid)
        {
            _logger.LogError("[PostSyncService] Failed to sync posts to Elasticsearch: {Error}", bulkIndexResponse.OriginalException?.Message);
        }
        else
        {
            _logger.LogInformation("[PostSyncService] Successfully synced {Count} posts to Elasticsearch.", posts.Count);
        }
    }

    public async Task ManuallySyncPostsAsync(CancellationToken cancellationToken)
    {
        var posts = await GetPosts(cancellationToken);
        await SyncPosts(posts, cancellationToken);
    }

    private async Task<List<PostDTO>> GetPosts(CancellationToken cancellationToken)
    {
        var (vectorDatabaseContext, mediator) = GetServices();

        var postsResult = await mediator.Send(new SyncAllPostsQuery(), cancellationToken);

        if (postsResult.IsSuccess)
        {
            return postsResult.Value;
        }
        else
        {
            _logger.LogError("Failed to get posts from API.");
            return new List<PostDTO>();
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("[PostSyncService] Service is starting.");

        _timer = new Timer(async _ => await ExecuteSyncTask(cancellationToken), null, TimeSpan.Zero, TimeSpan.FromMinutes(SyncInterval));

        await Task.CompletedTask;
    }

    private async Task ExecuteSyncTask(CancellationToken cancellationToken)
    {
        try
        {
            var posts = await GetPosts(cancellationToken);
            await SyncPosts(posts, cancellationToken);

            _logger.LogInformation("[PostSyncService] Running scheduled task.");
        }
        catch (Exception ex)
        {
            _logger.LogError("[PostSyncService] Error during scheduled task execution: {Error}", ex.Message);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("[PostSyncService] Service is stopping.");

        _timer?.Dispose();

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _logger.LogInformation("[PostSyncService] Service is stopping.");
    }
}
