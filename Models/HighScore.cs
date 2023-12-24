using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PhotoTaggingApi.Models;

public class HighScore
{
    public decimal Time { get; set; }
}