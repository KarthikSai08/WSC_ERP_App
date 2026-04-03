using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace WSC.Store.Infrastructure.Persistence.Context
{
    public class DapperContext
    {
        private readonly string _connectionString;
        public DapperContext(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
        }
        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
