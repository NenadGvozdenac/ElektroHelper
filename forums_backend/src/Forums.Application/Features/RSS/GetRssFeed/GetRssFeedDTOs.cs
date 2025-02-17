namespace forums_backend.src.Forums.Application.Features.RSS.GetRssFeed;

public record Author(string Id, string Username, string Email);
public record Forum(Guid Id, string Name);
public record RssItem(Guid Id, string Title, string Content, string Link, Author Author, Forum Forum, DateTime PublishedAt);
public record RssFeed(string Title, string Description, string Link, List<RssItem> Items);