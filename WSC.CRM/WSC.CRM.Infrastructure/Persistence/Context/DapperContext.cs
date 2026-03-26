using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace WSC.CRM.Infrastructure.Persistence.Context
{
    public class DapperContext
    {
        private readonly string _conString;
        private readonly IConfiguration _config;

        public DapperContext(string conString, IConfiguration config)
        {
            _conString = _config.GetConnectionString("DefaultConnection");
            _config = config;
        }
        public IDbConnection CreateConnection() => new SqlConnection(_conString);
    }
}
