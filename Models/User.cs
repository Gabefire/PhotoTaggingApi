using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PhotoTaggingApi.Models;

[BsonIgnoreExtraElements]
public class User
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonRequired]
    public string Username { get; set; } = string.Empty;

    [BsonRequired]
    public string PasswordHash { get; set; } = string.Empty;
}