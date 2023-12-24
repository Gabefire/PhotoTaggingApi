namespace PhotoTaggingApi.Models;

public class PhotoTaggingDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string TagsCollectionName { get; set; } = null!;

    public string HighScoresCollectionName { get; set; } = null!;

    public string UsersCollectionName { get; set; } = null!;
}