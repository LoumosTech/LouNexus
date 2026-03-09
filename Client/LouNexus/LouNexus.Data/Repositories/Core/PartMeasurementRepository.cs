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
    public class PartMeasurementRepository : IPartMeasurementRepository
    {
        // define the connection provider.
        private readonly IDbConnectionProvider _connectionProvider;

        public PartMeasurementRepository(IDbConnectionProvider connectionProvider)
        {
            // assign the connection provider.
            _connectionProvider = connectionProvider;
        }

        // implement the GetAllAsync method to retrieve all part measurements from the database.
        public async Task<IEnumerable<PartMeasurement>> GetAllAsync()
        {
            // create a list to hold the part measurements.
            var partMeasurements = new List<PartMeasurement>();

            // create a connection to the database using the connection provider.
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection, if not throw an exception.
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection.
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query.
            using var command = dbConnection.CreateCommand();

            // set the command text to select all part measurements from the database.
            command.CommandText = @"
                SELECT 
                    part_measurement_id,
                    part_measurement_name,
                    is_active,
                    created_utc
                FROM core.part_measurement";

            // execute the command and read the results.
            using var reader = await command.ExecuteReaderAsync();

            // loop through the results and add each part measurement to the list.
            while (await reader.ReadAsync())
            {
                // create a new PartMeasurement object and populate it with the data from the reader, then add it to the list.
                partMeasurements.Add(new PartMeasurement
                {
                    PartMeasurementId = reader.GetInt32(reader.GetOrdinal("part_measurement_id")),
                    PartMeasurementName = reader.GetString(reader.GetOrdinal("part_measurement_name")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                });
            }

            //return the list of part measurements.
            return partMeasurements;
        }

        // implement the GetByIdAsync method to retrieve a part measurement by its ID from the database.
        public async Task<PartMeasurement?> GetByIdAsync(int id)
        {
            // create a connection to the database using the connection provider.
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection, if not throw an exception.
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection.
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query.
            using var command = dbConnection.CreateCommand();

            // set the command text to select a part measurement by its ID from the database.
            command.CommandText = @"
                SELECT 
                    part_measurement_id,
                    part_measurement_name,
                    is_active,
                    created_utc
                FROM core.part_measurement
                WHERE part_measurement_id = @id";

            // create a parameter for the ID and add it to the command.
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            // execute the command and read the results.
            using var reader = await command.ExecuteReaderAsync();

            // loop through the results and return the part measurement if found, otherwise return null.
            while (await reader.ReadAsync())
            {
                // create a new PartMeasurement object and populate it with the data from the reader, then return it.
                return new PartMeasurement
                {
                    PartMeasurementId = reader.GetInt32(reader.GetOrdinal("part_measurement_id")),
                    PartMeasurementName = reader.GetString(reader.GetOrdinal("part_measurement_name")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                };
            }

            // if no part measurement is found, return null.
            return null;
        }

        // implement the InsertAsync method to insert a new part measurement into the database and return the new ID.
        public async Task<int> InsertAsync(PartMeasurement partMeasurement)
        {
            // create a connection to the database using the connection provider.
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection, if not throw an exception.
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection.
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query.
            using var command = dbConnection.CreateCommand();

            // set the command text to insert a new part measurement into the database and return the new ID.
            command.CommandText = @"
                INSERT INTO core.part_measurement (part_measurement_name, is_active, created_utc)
                VALUES (@name, @isActive, @createdUtc)
                RETURNING part_measurement_id";

            // create parameters for the part measurement properties and add them to the command.
            var nameParameter = command.CreateParameter();
            nameParameter.ParameterName = "@name";
            nameParameter.Value = partMeasurement.PartMeasurementName;
            command.Parameters.Add(nameParameter);

            var isActiveParameter = command.CreateParameter();
            isActiveParameter.ParameterName = "@isActive";
            isActiveParameter.Value = partMeasurement.IsActive;
            command.Parameters.Add(isActiveParameter);

            var createdUtcParameter = command.CreateParameter();
            createdUtcParameter.ParameterName = "@createdUtc";
            createdUtcParameter.Value = partMeasurement.CreatedUtc;
            command.Parameters.Add(createdUtcParameter);

            // execute the command and retrieve the new ID.
            object? result = await command.ExecuteScalarAsync();

            // check if the result is null or DBNull, if so throw an exception.
            if (result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException("Failed to insert PartMeasurement and retrieve the new ID.");
            }

            // return the new ID as an integer.
            return Convert.ToInt32(result);
        }

        // implement the UpdateAsync method to update an existing part measurement in the database and return a boolean indicating success.
        public async Task<bool> UpdateAsync(PartMeasurement partMeasurement)
        {
            // create a connection to the database using the connection provider.
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection, if not throw an exception.
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection.
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query.
            using var command = dbConnection.CreateCommand();

            // set the command text to update an existing part measurement in the database.
            command.CommandText = @"
                UPDATE core.part_measurement
                SET part_measurement_name = @name,
                    is_active = @isActive,
                    created_utc = @createdUtc
                WHERE part_measurement_id = @id";

            // create parameters for the part measurement properties and add them to the command.
            var nameParameter = command.CreateParameter();
            nameParameter.ParameterName = "@name";
            nameParameter.Value = partMeasurement.PartMeasurementName;
            command.Parameters.Add(nameParameter);

            var isActiveParameter = command.CreateParameter();
            isActiveParameter.ParameterName = "@isActive";
            isActiveParameter.Value = partMeasurement.IsActive;
            command.Parameters.Add(isActiveParameter);

            var createdUtcParameter = command.CreateParameter();
            createdUtcParameter.ParameterName = "@createdUtc";
            createdUtcParameter.Value = partMeasurement.CreatedUtc;
            command.Parameters.Add(createdUtcParameter);

            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = partMeasurement.PartMeasurementId;
            command.Parameters.Add(idParameter);

            // execute the command and check how many rows were affected.
            int rowsAffected = await command.ExecuteNonQueryAsync();

            // return true if at least one row was affected, indicating the update was successful, otherwise return false.
            return rowsAffected > 0;
        }

        // implement the DeleteAsync method to delete a part measurement by its ID from the database and return a boolean indicating success.
        public async Task<bool> DeleteAsync(int id)
        {
            // create a connection to the database using the connection provider.
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection, if not throw an exception.
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection.
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query.
            var command = dbConnection.CreateCommand();

            // set the command text to delete a part measurement by its ID from the database.
            command.CommandText = @"
                DELETE FROM core.part_measurement
                WHERE part_measurement_id = @id";

            // create a parameter for the ID and add it to the command.
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            // execute the command and check how many rows were affected.
            int rowsAffected = await command.ExecuteNonQueryAsync();

            // return true if at least one row was affected, indicating the delete was successful, otherwise return false.
            return rowsAffected > 0;
        }
    }
}
