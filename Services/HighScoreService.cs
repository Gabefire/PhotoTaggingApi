using PhotoTaggingApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace PhotoTaggingApi.Services;

public class HighScoreService
{
    private readonly IMongoCollection<HighScore> _highScoresCollection;


    public HighScoreService(
        IOptions<PhotoTaggingDatabaseSettings> photoTaggingDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            photoTaggingDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            photoTaggingDatabaseSettings.Value.DatabaseName);

        _highScoresCollection = mongoDatabase.GetCollection<HighScore>(
            photoTaggingDatabaseSettings.Value.HighScoresCollectionName);
    }

    public async Task<List<HighScore>> GetAsync() =>
        await _highScoresCollection.Find(_ => true).ToListAsync();

    public async Task CreateAsync(HighScore newHighScore) =>
        await _highScoresCollection.InsertOneAsync(newHighScore);

}