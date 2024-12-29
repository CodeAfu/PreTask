using FileManager.Models;
using MongoDB.Driver;

namespace FileManager.Data;

public class MongoDbService
{
    private readonly IConfiguration _configuration;
    private readonly IMongoDatabase _database;

    public MongoDbService(IConfiguration configuration)
    {
        _configuration = configuration;

        var connectionString = _configuration.GetConnectionString("Default");
        var mongoUrl = MongoUrl.Create(connectionString);
        var mongoClient = new MongoClient(mongoUrl);

        _database = mongoUrl.DatabaseName != null 
            ? mongoClient.GetDatabase(mongoUrl.DatabaseName) 
            : mongoClient.GetDatabase("file_manager");
    }

    public IMongoCollection<DataFile> getDataFilesCollection()
    {
        return _database.GetCollection<DataFile>("files");
    }

    public IMongoDatabase Database => _database;
}
