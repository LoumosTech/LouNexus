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
        //define the connection provider
        private readonly IDbConnectionProvider _connectionProvider;

        public FactoryRepository(IDbConnectionProvider connectionProvider)
        {
            //assign the connection provider
            _connectionProvider = connectionProvider;
        }

        //implement the GetAllAsync method to retrieve all factories from the database
        public async Task<IEnumerable<Factory>> GetAllAsync()
        {
            //create a list to hold the factories.
            var factories = new List<Factory>();

            //create a connection to the database
            using var connection = _connectionProvider.CreateConnection();

            //check if the connection is a DbConnection
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            //open the database connection
            await dbConnection.OpenAsync();

            //create a command to execute the SQL query
            using var command = dbConnection.CreateCommand();

            //set the command text to select all factories from the database
            command.CommandText = @"
                SELECT 
                    factory_id,
                    factory_name,
                    is_active,
                    created_utc
                FROM core.factory";

            //execute the command and read the results
            using var reader = await command.ExecuteReaderAsync();

            // loop through the results and add each factory to the list
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

            // return the list of factories
            return factories;
        }

        //implement the GetByIdAsync method to retrieve a factory by its ID from the database
        public async Task<Factory?> GetByIdAsync(int id)
        {
            //create a connection to the database
            using var connection = _connectionProvider.CreateConnection();

            //check if the connection is a DbConnection
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            //open the database connection
            await dbConnection.OpenAsync();

            //create a command to execute the SQL query
            using var command = dbConnection.CreateCommand();

            //set the command text to select a factory by its ID from the database
            command.CommandText = @"
                SELECT 
                    factory_id,
                    factory_name,
                    is_active,
                    created_utc
                FROM core.factory
                WHERE factory_id = @id";

            //create a parameters for the factory and add them to the command
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.Value = id;
            command.Parameters.Add(parameter);

            //execute the command and read the results
            using var reader = await command.ExecuteReaderAsync();

            // if a factory is found, return it, otherwise return null
            if (await reader.ReadAsync())
            {
                // create a new factory object and populate it with the data from the database
                return new Factory
                {
                    FactoryId = reader.GetInt32(reader.GetOrdinal("factory_id")),
                    FactoryName = reader.GetString(reader.GetOrdinal("factory_name")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                };
            }

            // if no factory is found, return null
            return null;
        }

        //implement the InsertAsync method to insert a new factory into the database and return the new factory ID
        public async Task<int> InsertAsync(Factory factory)
        {
            //create a connection to the database
            using var connection = _connectionProvider.CreateConnection();

            //check if the connection is a DbConnection
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            //open the database connection
            await dbConnection.OpenAsync();

            //create a command to execute the SQL query
            using var command = dbConnection.CreateCommand();

            //set the command text to insert a new factory into the database and return the new factory ID
            command.CommandText = @"
                INSERT INTO core.factory (factory_name, is_active, created_utc)
                VALUES (@name, @isActive, @createdUtc)
                RETURNING factory_id";

            //create parameters for the factory and add them to the command
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

            //execute the command and retrieve the new factory ID
            object? result = await command.ExecuteScalarAsync();

            // if the result is null or DBNull, throw an exception
            if (result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException("Failed to insert factory and retrieve the new ID.");
            }

            // return the new factory ID as an integer
            return Convert.ToInt32(result);
        }

        //implement the UpdateAsync method to update an existing factory in the database and return a boolean indicating success
        public async Task<bool> UpdateAsync(Factory factory)
        {
            //create a connection to the database
            using var connection = _connectionProvider.CreateConnection();

            //check if the connection is a DbConnection
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            //open the database connection
            await dbConnection.OpenAsync();

            //create a command to execute the SQL query
            using var command = dbConnection.CreateCommand();

            //set the command text to update an existing factory in the database
            command.CommandText = @"
                UPDATE core.factory
                SET factory_name = @name,
                    is_active = @isActive,
                    created_utc = @createdUtc
                WHERE factory_id = @id";

            //create parameters for the factory and add them to the command
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

            //execute the command and check how many rows were affected
            int rowsAffected = await command.ExecuteNonQueryAsync();

            // if at least one row was affected, the update was successful, so return true, otherwise return false
            return rowsAffected > 0;
        }

        //implement the DeleteAsync method to delete a factory from the database by its ID and return a boolean indicating success
        public async Task<bool> DeleteAsync(int id)
        {
            //create a connection to the database
            using var connection = _connectionProvider.CreateConnection();

            //check if the connection is a DbConnection
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            //open the database connection
            await dbConnection.OpenAsync();

            //create a command to execute the SQL query
            using var command = dbConnection.CreateCommand();

            //set the command text to delete a factory from the database by its ID
            command.CommandText = @"
                DELETE FROM core.factory
                WHERE factory_id = @id";

            //create a parameter for the factory ID and add it to the command
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            //execute the command and check how many rows were affected
            int rowsAffected = await command.ExecuteNonQueryAsync();

            //if at least one row was affected, the delete was successful, so return true, otherwise return false
            return rowsAffected > 0;
        }
    }
}
