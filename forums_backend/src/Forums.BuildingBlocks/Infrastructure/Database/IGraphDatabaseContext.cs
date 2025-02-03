using Neo4j.Driver;

namespace forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;

public interface IGraphDatabaseContext : IDisposable
{
    public Task<IResultCursor> RunAsync(string query, object parameters);
    public Task<IResultCursor> RunAsync(string query);
}