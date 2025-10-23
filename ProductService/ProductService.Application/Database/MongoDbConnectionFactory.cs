using MongoDB.Driver;
using ProductService.Application.Models;

namespace ProductService.Application.Database;

public class MongoDbConnectionFactory(string connectionString, string database)
{
    public readonly IMongoDatabase Database =
        new MongoClient(connectionString).GetDatabase(database);

    public readonly string ProductCollection = nameof(Product);
}