using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using ProductService.Application.Database;
using ProductService.Application.Models;
using ProductService.Application.Repositories;

namespace ProductService.Application;

public static class ApplicationServiceCollectionExtension
{
    public static IServiceCollection AddApplication(
        this IServiceCollection service,
        string connectionString,
        string database)
    {
        service.AddScoped(_ =>
            new MongoDbConnectionFactory(connectionString, database));
        service.AddScoped<IProductRepository, ProductRepository>();

        BsonSerializer.RegisterSerializer(
            new GuidSerializer(GuidRepresentation.Standard));

        BsonClassMap.RegisterClassMap<Product>(classMap =>
        {
            classMap.AutoMap();
            classMap.MapMember(x => x.ExpirationDate).SetIgnoreIfDefault(true);
            classMap.MapMember(x => x.Provider).SetIgnoreIfDefault(true);
            classMap.MapMember(x => x.Stock).SetIgnoreIfDefault(true);
            classMap.MapMember(x => x.SellingSeason).SetIgnoreIfDefault(true);
        });

        return service;
    }
}