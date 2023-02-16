using System.Linq.Expressions;
using GenericRepository;
using MongoDB.Driver;

namespace MongoRepository;

public class MongoRepository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : IEntity<TKey>
{
    private readonly IMongoCollection<TEntity> _database;
    private readonly FilterDefinitionBuilder<TEntity> _filterBuilder = Builders<TEntity>.Filter;

    public MongoRepository(IMongoDatabase database, string collectionName)
    {
        _database = database.GetCollection<TEntity>(collectionName);
    }

    public async Task<IReadOnlyCollection<TEntity>> GetAllAsync()
    {
        return await _database.Find(_filterBuilder.Empty).ToListAsync();
    }

    public async Task<IReadOnlyCollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _database.Find(filter).ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(TKey id)
    {
        var filter = _filterBuilder.Eq(entity => entity.Id, id);

        return await _database.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _database.Find(filter).FirstOrDefaultAsync();
    }

    public async Task PostAsync(TEntity entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        await _database.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var filter = _filterBuilder.Eq(e => e.Id, entity.Id);
        await _database.ReplaceOneAsync(filter, entity);
    }

    public async Task DeleteAsync(TKey id)
    {
        var filter = _filterBuilder.Eq(e => e.Id, id);

        await _database.DeleteOneAsync(filter);
    }
}