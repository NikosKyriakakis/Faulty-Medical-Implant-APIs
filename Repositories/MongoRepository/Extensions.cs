using GenericRepository;
using GenericRepository.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoRepository.Settings;

namespace MongoRepository;

public static class Extensions
{
    public static IServiceCollection AddMongo(this IServiceCollection services)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

        services.AddSingleton(serviceProvider =>
        {
            var configuration = serviceProvider.GetService<IConfiguration>() ?? throw new Exception("IConfiguration object is null");
            var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>() 
                                  ?? throw new Exception("Service settings object is null");
            var mongoDbSettings = configuration.GetSection(nameof(MongoSettings)).Get<MongoSettings>() 
                                  ?? throw new Exception("Mongo DB settings object is null");
            var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
            return mongoClient.GetDatabase(serviceSettings.ServiceName);
        });

        return services;
    }

    public static IServiceCollection AddMongoRepository<TEntity, TKey>(this IServiceCollection services, string collectionName)
        where TEntity : IEntity<TKey>
    {
        services.AddSingleton<IRepository<TEntity, TKey>>(serviceProvider =>
        {
            var database = serviceProvider.GetService<IMongoDatabase>() ?? throw new Exception("Database object is null");
            return new MongoRepository<TEntity, TKey>(database, collectionName);
        });

        return services;
    }
}