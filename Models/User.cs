using MongoDB.Bson.Serialization.Attributes;

namespace PhotoTaggingApi.Models;

public class User
{
    [BsonRequired]
    public string UserName { get; set; } = null!;

    [BsonRequired]
    public string Password { get; set; } = null!;
}