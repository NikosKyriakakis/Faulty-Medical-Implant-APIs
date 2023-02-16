using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace SqlRepository;

public sealed class AppDbContext<TEntity, TKey> : DbContext
    where TEntity : class, IEntity<TKey>
{
    public AppDbContext(DbContextOptions<AppDbContext<TEntity, TKey>> dbContextOptions) : base(dbContextOptions)
    {
        try
        {
            if (Database.GetService<IDatabaseCreator>() is not RelationalDatabaseCreator databaseCreator) return;
            if (!databaseCreator.CanConnect()) databaseCreator.Create();
            if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
        }
        catch (Exception exception)
        {
            throw new Exception(exception.Message);
        }     
    }
    
    public DbSet<TEntity>? Elements { get; set; }
}