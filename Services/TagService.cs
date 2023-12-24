using PhotoTaggingApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Tag = PhotoTaggingApi.Models.Tag;
using BookStoreApi.Models;

namespace PhotoTaggingApi.Services;

public class TagsService
{
    private readonly IMongoCollection<Tag> _tagsCollection;


    public TagsService(
        IOptions<PhotoTaggingDatabaseSettings> photoTaggingDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            photoTaggingDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            photoTaggingDatabaseSettings.Value.DatabaseName);

        _tagsCollection = mongoDatabase.GetCollection<Tag>(
            photoTaggingDatabaseSettings.Value.TagsCollectionName);
    }

    public async Task<List<Tag>> GetAsync() =>
        await _tagsCollection.Find(_ => true).ToListAsync();
}