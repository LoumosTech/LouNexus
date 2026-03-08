using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;

namespace LouNexus.Data.DataBase
{
    public class DbConnectionProvider : IDbConnectionProvider
    {
        private readonly string _connectionString;

        public DbConnectionProvider(string connectionString)
        {
            _connectionString = connectionString;
        }


        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
