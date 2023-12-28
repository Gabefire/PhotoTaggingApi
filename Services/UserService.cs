using PhotoTaggingApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace PhotoTaggingApi.Services;

public class UsersService
{
    private readonly IMongoCollection<User> _usersCollection;


    public UsersService(
        IOptions<PhotoTaggingDatabaseSettings> photoTaggingDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            photoTaggingDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            photoTaggingDatabaseSettings.Value.DatabaseName);

        _usersCollection = mongoDatabase.GetCollection<User>(
            photoTaggingDatabaseSettings.Value.UsersCollectionName);
    }

    public async Task CreateAsync(User newUser) =>
        await _usersCollection.InsertOneAsync(newUser);

    public async Task<User> GetAsync(string username) =>
        await _usersCollection.Find(x => x.Username == username).FirstOrDefaultAsync();

    public async Task<User> GetAsyncId(string userId) =>
        await _usersCollection.Find(x => x.Id.ToString() == userId).FirstOrDefaultAsync();

}