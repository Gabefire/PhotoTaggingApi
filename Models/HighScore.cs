using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace PhotoTaggingApi.Models;

public class HighScore
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonRequired]
    public double Time { get; set; }

    public string UserId { get; set; } = string.Empty;
}