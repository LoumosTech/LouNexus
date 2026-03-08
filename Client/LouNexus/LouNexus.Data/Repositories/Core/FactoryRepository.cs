using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LouNexus.Core.Interfaces.Core;
using LouNexus.Core.Models.Core;
using LouNexus.Data.DataBase;

namespace LouNexus.Data.Repositories.Core
{
    public class FactoryRepository : IFactoryRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;

        public FactoryRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public async Task<IEnumerable<Factory>> GetAllAsync()
        {
            var factories = new List<Factory>();
            using var connection = _connectionProvider.CreateConnection();

            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            await dbConnection.OpenAsync();

            using var command = dbConnection.CreateCommand();

            command.CommandText = @"
                SELECT 
                    factory_id,
                    factory_name,
                    is_active,
                    created_utc
                FROM core.factory";

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                factories.Add(new Factory
                {
                    FactoryId = reader.GetInt32(reader.GetOrdinal("factory_id")),
                    FactoryName = reader.GetString(reader.GetOrdinal("factory_name")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                });
            }

            return factories;
        }

        public async Task<Factory?> GetByIdAsync(int id)
        {
            using var connection = _connectionProvider.CreateConnection();

            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            await dbConnection.OpenAsync();

            using var command = dbConnection.CreateCommand();

            command.CommandText = @"
                SELECT 
                    factory_id,
                    factory_name,
                    is_active,
                    created_utc
                FROM core.factory
                WHERE factory_id = @id";

            var parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.Value = id;
            command.Parameters.Add(parameter);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Factory
                {
                    FactoryId = reader.GetInt32(reader.GetOrdinal("factory_id")),
                    FactoryName = reader.GetString(reader.GetOrdinal("factory_name")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                };
            }

            return null;
        }

        public async Task<int> InsertAsync(Factory factory)
        {
            using var connection = _connectionProvider.CreateConnection();

            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            await dbConnection.OpenAsync();

            using var command = dbConnection.CreateCommand();

            command.CommandText = @"
                INSERT INTO core.factory (factory_name, is_active, created_utc)
                VALUES (@name, @isActive, @createdUtc)
                RETURNING factory_id";

            var nameparameter = command.CreateParameter();
            nameparameter.ParameterName = "@name";
            nameparameter.Value = factory.FactoryName;
            command.Parameters.Add(nameparameter);

            var isActiveParameter = command.CreateParameter();
            isActiveParameter.ParameterName = "@isActive";
            isActiveParameter.Value = factory.IsActive;
            command.Parameters.Add(isActiveParameter);

            var createdUtcParameter = command.CreateParameter();
            createdUtcParameter.ParameterName = "@createdUtc";
            createdUtcParameter.Value = factory.CreatedUtc;
            command.Parameters.Add(createdUtcParameter);

            object? result = await command.ExecuteScalarAsync();

            if (result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException("Failed to insert factory and retrieve the new ID.");
            }

            return Convert.ToInt32(result);
        }

        public async Task<bool> UpdateAsync(Factory factory)
        {
            using var connection = _connectionProvider.CreateConnection();

            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            await dbConnection.OpenAsync();

            using var command = dbConnection.CreateCommand();

            command.CommandText = @"
                UPDATE core.factory
                SET factory_name = @name,
                    is_active = @isActive,
                    created_utc = @createdUtc
                WHERE factory_id = @id";

            var nameParameter = command.CreateParameter();
            nameParameter.ParameterName = "@name";
            nameParameter.Value = factory.FactoryName;
            command.Parameters.Add(nameParameter);

            var isActiveParameter = command.CreateParameter();
            isActiveParameter.ParameterName = "@isActive";
            isActiveParameter.Value = factory.IsActive;
            command.Parameters.Add(isActiveParameter);

            var createdUtcParameter = command.CreateParameter();
            createdUtcParameter.ParameterName = "@createdUtc";
            createdUtcParameter.Value = factory.CreatedUtc;
            command.Parameters.Add(createdUtcParameter);

            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = factory.FactoryId;
            command.Parameters.Add(idParameter);

            int rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _connectionProvider.CreateConnection();

            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            await dbConnection.OpenAsync();

            using var command = dbConnection.CreateCommand();

            command.CommandText = @"
                DELETE FROM core.factory
                WHERE factory_id = @id";

            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            int rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }
    }
}
