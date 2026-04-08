using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using WSC.Store.Application.Interfaces.RepositoryInterfaces;

namespace WSC.Store.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _connection;
        public IDbTransaction Transaction { get; private set; }
        public IDbConnection Connection => _connection;
        public UnitOfWork(IDbConnection connection) => _connection = connection;

        public async Task BeginAsync()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            Transaction = _connection.BeginTransaction();
        }
        public  Task CommitAsync()
        {
            Transaction?.Commit();
            Dispose();
            return Task.CompletedTask;
        }

        public Task RollbackAsync()
        {
            Transaction?.Rollback();
            Dispose();
            return Task.CompletedTask;
        }

        public void  Dispose()
        {
            Transaction?.Dispose();
            _connection?.Close();
        }
    }
}
