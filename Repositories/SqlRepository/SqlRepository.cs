using System.Linq.Expressions;
using GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace SqlRepository;

public class SqlRepository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    private readonly AppDbContext<TEntity, TKey> _context;

    public SqlRepository(AppDbContext<TEntity, TKey> context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public async Task<IReadOnlyCollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _context.Set<TEntity>().Where(filter).ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(TKey id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _context.Set<TEntity>().Where(filter).FirstOrDefaultAsync();
    }

    public async Task PostAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
        
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        if (_context.Elements != null)
        {
            _context.Elements.Update(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(TKey id)
    {
        var entity = await _context.Set<TEntity>().FindAsync(id);
        if (entity is null)
            return;
        
        _context.Set<TEntity>().Remove(entity);
        
        await _context.SaveChangesAsync();
    }
}