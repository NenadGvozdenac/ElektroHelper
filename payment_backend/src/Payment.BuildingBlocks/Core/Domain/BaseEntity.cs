using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace payment_backend.src.Payment.BuildingBlocks.Core.Domain;

public abstract class BaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
}