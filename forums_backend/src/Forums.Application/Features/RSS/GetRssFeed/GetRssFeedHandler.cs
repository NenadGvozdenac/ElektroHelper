using forums_backend.src.Forums.BuildingBlocks.Core.Domain;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure;
using forums_backend.src.Forums.BuildingBlocks.Infrastructure.Database;
using MediatR;
using Neo4j.Driver;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;

namespace forums_backend.src.Forums.Application.Features.RSS.GetRssFeed;

public class GetRssFeedHandler(IGraphDatabaseContext context) : IRequestHandler<GetRssFeedQuery, Result<string>>
{
    private static string GeneratePostLink(Guid guid) => $"http://localhost:9090/posts/{guid}";
    private static string GenerateFeedLink() => "http://localhost:9090/rss";
    public async Task<Result<string>> Handle(GetRssFeedQuery request, CancellationToken cancellationToken)
    {
        var query = @"
            MATCH (f:Forum)-[:HAS_POST]->(p:Post)<-[:POSTED]-(u:User)
            ORDER BY p.createdAt DESC
            LIMIT 15
            RETURN f, p, u
        ";

        var resultCursor = await context.RunAsync(query);
        var result = await resultCursor.ToListAsync(cancellationToken);

        var items = result.Select(record =>
        {
            var postNode = record["p"].As<INode>();
            var userNode = record["u"].As<INode>();

            var title = postNode["title"].As<string>();
            var content = postNode["content"].As<string>();
            var link = new Uri(GeneratePostLink(Guid.Parse(postNode["id"].As<string>())));
            var publishDate = postNode["createdAt"].As<string>().FromNeo4jDateTime();
            var author = userNode["username"].As<string>();

            return new SyndicationItem(title, content, link)
            {
                PublishDate = publishDate,
                Authors = { new SyndicationPerson("", author, "") }
            };
        }).ToList();

        var feed = new SyndicationFeed("Elektrohelper RSS Feed", "RSS Feed for Elektrohelper", new Uri(GenerateFeedLink()))
        {
            Items = items
        };

        using var stream = new MemoryStream();
        using var writer = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true });

        new Rss20FeedFormatter(feed).WriteTo(writer);
        writer.Flush();

        var rssString = Encoding.UTF8.GetString(stream.ToArray());

        return Result<string>.Success(rssString);
    }
}
