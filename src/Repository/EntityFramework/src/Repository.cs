using System.Linq.Expressions;
using Ecubytes.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Ecubytes.Repository.EntityFramework;

public class Repository<T> : IRepository<T> where T : class
{    
    protected DbSet<T> dbSet;
    
    public Repository(DbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);        
        this.dbSet = context.Set<T>();
    }

    /// <summary>
    /// Gets the IQueryable representation for the current repository.
    /// </summary>
    /// <returns>An IQueryable instance.</returns>
    /// <param name="filter">A function to filter the query.</param>
    /// <param name="orderBy">A function to order the query.</param>
    /// <param name="includeProperties">A comma-separated list of properties to include in the query.</param>
    public IQueryable<T> GetQueryable(
        Expression<Func<T, bool>>? filter,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string? includeProperties = null)
    {
        IQueryable<T> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        IQueryable<T> result = query;

        if (orderBy != null)
        {
            result = orderBy(query);
        }

        return result;
    }
    /// <summary>
    /// Get all entities of type T.
    /// </summary>
    /// <returns>A list of all entities.</returns>
    /// <param name="filter">A function to filter the query.</param>
    /// <param name="orderBy">A function to order the query.</param>
    /// <param name="includeProperties">A comma-separated list of properties to include in the query.</param>
    public async Task<IEnumerable<T>> GetAsync(
        Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>,
        IOrderedQueryable<T>>? orderBy = null,
        string? includeProperties = null)
    {
        IQueryable<T> queryable = GetQueryable(filter, orderBy, includeProperties);

        return await queryable.ToListAsync();
    }

    /// <summary>
    /// Get the first entity of type T.
    /// </summary>
    /// <returns>The first entity.</returns>
    /// <param name="filter">A function to filter the query.</param>
    /// <param name="includeProperties">A comma-separated list of properties to include in the query.</param>
    public Task<T?> GetFirstOrDefaultAsync(
        Expression<Func<T, bool>>? filter = null,
        string? includeProperties = null)
    {
        IQueryable<T> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        return query.FirstOrDefaultAsync();
    }

    /// <summary>
    /// Get the entity for the given primary key.
    /// </summary>
    /// <returns>The entity.</returns>
    /// <param name="keyValues">The key primary key values</param>
    public ValueTask<T?> GetAsync(params object[] keyValues) => dbSet.FindAsync(keyValues);

    /// <summary>
    /// Add the entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    public void Add(T entity)
    {
        dbSet.Add(entity);
    }

    /// <summary>
    /// Remove the entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to remove.</param>
    public void AddRange(IEnumerable<T> entities)
    {
        dbSet.AddRange(entities);
    }

    /// <summary>
    /// Update the entity in the repository.    
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    public void Update(T entity)
    {
        dbSet.Update(entity);
    }
    /// <summary>
    /// Update a range of entities in the repository.
    /// </summary>
    /// <param name="entities">The entities to update.</param>
    public void UpdateRange(IEnumerable<T> entities)
    {
        dbSet.UpdateRange(entities);
    }
    /// <summary>
    /// Remove the entity for the given primary key.
    /// </summary>
    /// <param name="keyValues">The primary key values.</param>
    public void Remove(params object[] keyValues)
    {
        T? entityToRemove = dbSet.Find(keyValues);
        if (entityToRemove != null)
            Remove(entityToRemove);
    }
    /// <summary>
    /// Remove the entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to remove.</param>
    public void Remove(T entity)
    {
        dbSet.Remove(entity);
    }
    /// <summary>
    /// Remove a range of entities from the repository.
    /// </summary>
    /// <param name="entities">The entities to remove.</param>
    public void RemoveRange(IEnumerable<T> entities)
    {
        dbSet.RemoveRange(entities);
    }
    /// <summary>
    /// Verify if the entity exists in the repository.
    /// </summary>
    /// <param name="filter">A function to filter the query.</param>
    public Task<bool> ExistsAsync(Expression<Func<T, bool>>? filter = null)
    {
        if(filter!=null)        
            return dbSet.AnyAsync(filter);
        else
            return dbSet.AnyAsync();
    }
}