namespace forums_backend.src.Forums.Application.BackgroundServices;

public interface IPostSyncService : IHostedService, IDisposable
{
    public Task ManuallySyncPostsAsync(CancellationToken cancellationToken);
}