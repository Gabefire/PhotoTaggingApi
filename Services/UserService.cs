using PhotoTaggingApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using BookStoreApi.Models;

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

    public async Task<User> GetAsync(string userName) =>
        await _usersCollection.Find(x => x.UserName == userName).FirstOrDefaultAsync();

}