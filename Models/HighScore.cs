using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace PhotoTaggingApi.Models;

public class HighScore
{
    [BsonRequired]
    public double Time { get; set; }

    public string UserId { get; set; } = string.Empty;
}