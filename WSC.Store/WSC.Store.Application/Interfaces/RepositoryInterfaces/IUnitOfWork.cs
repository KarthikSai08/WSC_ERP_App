using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

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
