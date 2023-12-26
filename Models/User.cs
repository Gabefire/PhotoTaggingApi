using MongoDB.Bson.Serialization.Attributes;

namespace PhotoTaggingApi.Models;

[BsonIgnoreExtraElements]
public class User
{
    [BsonRequired]
    public string Username { get; set; } = string.Empty;

    [BsonRequired]
    public string PasswordHash { get; set; } = string.Empty;
}