using System.Collections;
using Nest;

namespace forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;

public interface IVectorDatabaseContext
{
    public Task<BulkResponse> IndexData<T>(List<T> data, string indexName) where T : class;
    public Task<ISearchResponse<T>> QueryData<T>(string query, string indexName) where T : class;
}
