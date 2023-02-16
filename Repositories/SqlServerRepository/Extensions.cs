using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlRepository;

namespace SqlServerRepository;

public static class Extensions
{
    public static IServiceCollection AddSqlServer<TEntity, TKey>(
        this IServiceCollection services, 
        IConfiguration configuration, 
        string contextName)
        where TEntity : class, IEntity<TKey>
    {
        var connectionString = configuration.GetConnectionString(contextName);
        services.AddDbContext<AppDbContext<TEntity, TKey>>(options =>
        {
            options.UseSqlServer(connectionString);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
        return services;
    }

    public static IServiceCollection AddSqlServerRepository<TEntity, TKey>(this IServiceCollection services) 
        where TEntity : class, IEntity<TKey>
    {
        services.AddScoped<IRepository<TEntity, TKey>>(serviceProvider =>
        {
            var database = serviceProvider.GetService<AppDbContext<TEntity, TKey>>() 
                           ?? throw new Exception("Database object is null");
            return new SqlRepository<TEntity, TKey>(database);
        });
        return services;
    }
}