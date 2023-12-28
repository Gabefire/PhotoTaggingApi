using PhotoTaggingApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;


namespace PhotoTaggingApi.Services;

public class HighScoreService
{
    private readonly IMongoCollection<HighScore> _highScoresCollection;

    private readonly IMongoCollection<User> _usersCollection;
    public HighScoreService(
        IOptions<PhotoTaggingDatabaseSettings> photoTaggingDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            photoTaggingDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            photoTaggingDatabaseSettings.Value.DatabaseName);

        _highScoresCollection = mongoDatabase.GetCollection<HighScore>(
            photoTaggingDatabaseSettings.Value.HighScoresCollectionName);

        _usersCollection = mongoDatabase.GetCollection<User>(
            photoTaggingDatabaseSettings.Value.UsersCollectionName);
    }

    public async Task<List<HighScore>> GetAsync() =>
         await _highScoresCollection.Find(_ => true).SortBy(i => i.Time).Limit(10).ToListAsync();


    public async Task CreateAsync(HighScore newHighScore) =>
        await _highScoresCollection.InsertOneAsync(newHighScore);

}