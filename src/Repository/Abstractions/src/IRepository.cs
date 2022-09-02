using System.Linq.Expressions;

namespace Ecubytes.Repository.Abstractions;

public interface IRepository<T> where T : class
{
    ValueTask<T?> GetAsync(params object[] keyValues);
    IQueryable<T> GetQueryable(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string? includeProperties = null
    );        
    Task<IEnumerable<T>> GetAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string? includeProperties = null
    );
    Task<bool> ExistsAsync(
        Expression<Func<T, bool>>? filter = null
    );
    Task<T?> GetFirstOrDefaultAsync(
        Expression<Func<T, bool>>? filter = null,
        string? includeProperties = null
    );

    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    void Remove(params object[] keyValues);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);    
}