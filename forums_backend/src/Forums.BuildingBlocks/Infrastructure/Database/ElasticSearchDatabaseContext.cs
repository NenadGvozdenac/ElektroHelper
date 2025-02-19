using Nest;

namespace forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;

public class VectorDatabaseContext : IVectorDatabaseContext
{
    private readonly ElasticClient _elasticClient;

    public VectorDatabaseContext()
    {
        var settings = new ConnectionSettings(new Uri("http://elasticsearch:9200"))
            .BasicAuthentication("elastic", "password")
            .DefaultIndex("forum_posts");

        _elasticClient = new ElasticClient(settings);
    }

    public async Task<DeleteIndexResponse> DeleteIndex(string indexName)
    {
        return await _elasticClient.Indices.DeleteAsync(indexName);
    }

    public async Task<BulkResponse> IndexData<T>(List<T> data, string indexName) where T : class
    {
        var response = await _elasticClient.IndexManyAsync(data, indexName);
        return response;
    }
    public async Task<ISearchResponse<T>> QueryData<T>(string query, string indexName) where T : class
    {
        return await _elasticClient.SearchAsync<T>(s => s
            .Index(indexName)
            .Query(q => q
                .QueryString(qs => qs.Query(query))
            )
        );
    }
}