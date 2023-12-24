using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PhotoTaggingApi.Models;

public class Tag
{
    [BsonElement("Name"), BsonRequired]
    public string TagName { get; } = null!;

    [BsonRequired]
    public decimal MaxX { get; }

    [BsonRequired]
    public decimal MaxY { get; }

    [BsonRequired]
    public decimal MinX { get; }

    [BsonRequired]
    public decimal MinY { get; }
}