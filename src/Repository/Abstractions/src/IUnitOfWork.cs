namespace Ecubytes.Repository.Abstractions;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    Task ResilientTransactionAsync(Func<Task> action);
}