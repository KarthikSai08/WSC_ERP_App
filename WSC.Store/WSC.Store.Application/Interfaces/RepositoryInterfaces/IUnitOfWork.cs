using System.Data;

namespace WSC.Store.Application.Interfaces.RepositoryInterfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDbTransaction Transaction { get; }
        Task BeginAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
