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
    public class PartMeasurementSpecRepository : IPartMeasurementSpecRepository
    {
        // define connection provider
        private readonly IDbConnectionProvider _connectionProvider;
        

        public PartMeasurementSpecRepository(IDbConnectionProvider connectionProvider)
        {
            // assign connection provider
            _connectionProvider = connectionProvider;
        }

        // implement the GetAllAsync method to retrieve all PartMeasurementSpec records from the database
        public async Task<IEnumerable<PartMeasurementSpec>> GetAllAsync()
        {
            // create a list to hold the PartMeasurementSpecs.
            var partMeasurementSpecs = new List<PartMeasurementSpec>();

            // create a connection to the database.
            using var connection = _connectionProvider.CreateConnection();

            // check if connection is a DbConnection, if not throw an exception.
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection.
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query.
            using var command = dbConnection.CreateCommand();

            // set the command text to select all PartMeasurementSpec from the database.
            command.CommandText = @"
                SELECT 
                    part_measurement_spec_id, 
                    part_id, 
                    workstation_type_id, 
                    measurement_name, 
                    target_value, 
                    upper_limit, 
                    lower_limit, 
                    is_active, 
                    created_utc 
                FROM core.part_measurement_specs";

            // execute the command and read the results.
            using var reader = await command.ExecuteReaderAsync();

            // loop through the results and create PartMeasurementSpec objects, adding them to the list.
            while (await reader.ReadAsync())
            {
                var partMeasurementSpec = new PartMeasurementSpec
                {
                    PartMeasurementSpecId = reader.GetInt32(reader.GetOrdinal("part_measurement_spec_id")),
                    PartId = reader.GetInt32(reader.GetOrdinal("part_id")),
                    WorkStationTypeId = reader.GetInt32(reader.GetOrdinal("workstation_type_id")),
                    MeasurementName = reader.GetString(reader.GetOrdinal("measurement_name")),
                    TargetValue = reader.GetDecimal(reader.GetOrdinal("target_value")),
                    UpperLimit = reader.GetDecimal(reader.GetOrdinal("upper_limit")),
                    LowerLimit = reader.GetDecimal(reader.GetOrdinal("lower_limit")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                };
                partMeasurementSpecs.Add(partMeasurementSpec);
            }

            // return the list of PartMeasurementSpecs.
            return partMeasurementSpecs;

        }

        // implement the GetByIdAsync method to retrieve a PartMeasurementSpec record by its ID from the database
        public async Task<PartMeasurementSpec?> GetByIdAsync(int id)
        {
            // create a connection to the database.
            using var connection = _connectionProvider.CreateConnection();

            // check if connection is a DbConnection, if not throw an exception.
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection.
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query.
            using var command = dbConnection.CreateCommand();

            // set the command text to select a PartMeasurementSpec by ID from the database.
            command.CommandText = @"
                SELECT 
                    part_measurement_spec_id, 
                    part_id, 
                    workstation_type_id, 
                    measurement_name, 
                    target_value, 
                    upper_limit, 
                    lower_limit, 
                    is_active, 
                    created_utc 
                FROM core.part_measurement_specs
                WHERE part_measurement_spec_id = @id";

            // create a parameter for the ID and add it to the command.
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            // execute the command and read the result.
            using var reader = await command.ExecuteReaderAsync();

            // if a record is found, create a PartMeasurementSpec object and return it.
            if (await reader.ReadAsync())
            {
                var partMeasurementSpec = new PartMeasurementSpec
                {
                    PartMeasurementSpecId = reader.GetInt32(reader.GetOrdinal("part_measurement_spec_id")),
                    PartId = reader.GetInt32(reader.GetOrdinal("part_id")),
                    WorkStationTypeId = reader.GetInt32(reader.GetOrdinal("workstation_type_id")),
                    MeasurementName = reader.GetString(reader.GetOrdinal("measurement_name")),
                    TargetValue = reader.GetDecimal(reader.GetOrdinal("target_value")),
                    UpperLimit = reader.GetDecimal(reader.GetOrdinal("upper_limit")),
                    LowerLimit = reader.GetDecimal(reader.GetOrdinal("lower_limit")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                };
                return partMeasurementSpec;
            }

            // if no record is found, return null.
            return null;
        }

        // implement the InsertAsync method to insert a new PartMeasurementSpec record into the database
        public async Task<int> InsertAsync(PartMeasurementSpec partMeasurementSpec)
        {
            // create a connection to the database.
            using var connection = _connectionProvider.CreateConnection();

            // check if connection is a DbConnection, if not throw an exception.
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection.
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query.
            using var command = dbConnection.CreateCommand();

            // set the command text to insert a new PartMeasurementSpec into the database.
            command.CommandText = @"
                INSERT INTO core.part_measurement_specs 
                    (part_id, workstation_type_id, measurement_name, target_value, upper_limit, lower_limit, is_active, created_utc) 
                VALUES 
                    (@partId, @workStationTypeId, @measurementName, @targetValue, @upperLimit, @lowerLimit, @isActive, @createdUtc);
                RETURNING part_measurement_spec_id;";

            // create parameters for each property of the PartMeasurementSpec and add them to the command.
            var partIdParameter = command.CreateParameter();
            partIdParameter.ParameterName = "@partId";
            partIdParameter.Value = partMeasurementSpec.PartId;
            command.Parameters.Add(partIdParameter);

            var workStationTypeIdParameter = command.CreateParameter();
            workStationTypeIdParameter.ParameterName = "@workStationTypeId";
            workStationTypeIdParameter.Value = partMeasurementSpec.WorkStationTypeId;
            command.Parameters.Add(workStationTypeIdParameter);

            var measurementNameParameter = command.CreateParameter();
            measurementNameParameter.ParameterName = "@measurementName";
            measurementNameParameter.Value = partMeasurementSpec.MeasurementName;
            command.Parameters.Add(measurementNameParameter);

            var targetValueParameter = command.CreateParameter();
            targetValueParameter.ParameterName = "@targetValue";
            targetValueParameter.Value = partMeasurementSpec.TargetValue;
            command.Parameters.Add(targetValueParameter);

            var upperValueParameter = command.CreateParameter();
            upperValueParameter.ParameterName = "@upperLimit";
            upperValueParameter.Value = partMeasurementSpec.UpperLimit;
            command.Parameters.Add(upperValueParameter);

            var lowerValueParameter = command.CreateParameter();
            lowerValueParameter.ParameterName = "@lowerLimit";
            lowerValueParameter.Value = partMeasurementSpec.LowerLimit;
            command.Parameters.Add(lowerValueParameter);

            var isActiveParameter = command.CreateParameter();
            isActiveParameter.ParameterName = "@isActive";
            isActiveParameter.Value = partMeasurementSpec.IsActive;
            command.Parameters.Add(isActiveParameter);

            var createdUtcParameter = command.CreateParameter();
            createdUtcParameter.ParameterName = "@createdUtc";
            createdUtcParameter.Value = partMeasurementSpec.CreatedUtc;
            command.Parameters.Add(createdUtcParameter);

            // execute the command and return the new record ID.
            object? result = await command.ExecuteScalarAsync();

            if( result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException("Failed to insert the PartMeasurementSpec record.");
            }

            //return the new record ID.
            return Convert.ToInt32(result);
        }

        // implement the UpdateAsync method to update an existing PartMeasurementSpec record in the database
        public async Task<bool> UpdateAsync(PartMeasurementSpec partMeasurementSpec)
        {
            // create a connection to the database.
            using var connection = _connectionProvider.CreateConnection();

            // check if connection is a DbConnection, if not throw an exception.
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection.
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query.
            using var command = dbConnection.CreateCommand();

            // set the command text to update an existing PartMeasurementSpec in the database.
            command.CommandText = @"
                UPDATE core.part_measurement_specs
                SET 
                    part_id = @partId,
                    workstation_type_id = @workStationTypeId,
                    measurement_name = @measurementName,
                    target_value = @targetValue,
                    upper_limit = @upperLimit,
                    lower_limit = @lowerLimit,
                    is_active = @isActive,
                    created_utc = @createdUtc
                WHERE part_measurement_spec_id = @id";

            // create parameters for each property of the PartMeasurementSpec and add them to the command.
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = partMeasurementSpec.PartMeasurementSpecId;
            command.Parameters.Add(idParameter);

            var partIdParameter = command.CreateParameter();
            partIdParameter.ParameterName = "@partId";
            partIdParameter.Value = partMeasurementSpec.PartId;
            command.Parameters.Add(partIdParameter);

            var workStationTypeIdParameter = command.CreateParameter();
            workStationTypeIdParameter.ParameterName = "@workStationTypeId";
            workStationTypeIdParameter.Value = partMeasurementSpec.WorkStationTypeId;
            command.Parameters.Add(workStationTypeIdParameter);

            var measurementNameParameter = command.CreateParameter();
            measurementNameParameter.ParameterName = "@measurementName";
            measurementNameParameter.Value = partMeasurementSpec.MeasurementName;
            command.Parameters.Add(measurementNameParameter);

            var targetValueParameter = command.CreateParameter();
            targetValueParameter.ParameterName = "@targetValue";
            targetValueParameter.Value = partMeasurementSpec.TargetValue;
            command.Parameters.Add(targetValueParameter);

            var upperValueParameter = command.CreateParameter();
            upperValueParameter.ParameterName = "@upperLimit";
            upperValueParameter.Value = partMeasurementSpec.UpperLimit;
            command.Parameters.Add(upperValueParameter);

            var lowerValueParameter = command.CreateParameter();
            lowerValueParameter.ParameterName = "@lowerLimit";
            lowerValueParameter.Value = partMeasurementSpec.LowerLimit;
            command.Parameters.Add(lowerValueParameter);

            var isActiveParameter = command.CreateParameter();
            isActiveParameter.ParameterName = "@isActive";
            isActiveParameter.Value = partMeasurementSpec.IsActive;
            command.Parameters.Add(isActiveParameter);

            var createdUtcParameter = command.CreateParameter();
            createdUtcParameter.ParameterName = "@createdUtc";
            createdUtcParameter.Value = partMeasurementSpec.CreatedUtc;
            command.Parameters.Add(createdUtcParameter);

            // execute the command.
            int rowsAffected = await command.ExecuteNonQueryAsync();

            // return true if the update was successful, otherwise return false.
            return rowsAffected > 0;
        }

        // implement the DeleteAsync method to delete a PartMeasurementSpec record by its ID from the database
        public async Task<bool> DeleteAsync(int id)
        {
            // create a connection to the database.
            using var connection = _connectionProvider.CreateConnection();

            // check if connection is a DbConnection, if not throw an exception.
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection.
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query.
            using var command = dbConnection.CreateCommand();

            // set the command text to delete a PartMeasurementSpec by ID from the database.
            command.CommandText = @"
                DELETE FROM core.part_measurement_specs
                WHERE part_measurement_spec_id = @id";

            // create a parameter for the ID and add it to the command.
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            // execute the command.
            int rowsAffected = await command.ExecuteNonQueryAsync();

            // return true if the delete was successful, otherwise return false.
            return rowsAffected > 0;

            throw new NotImplementedException();
        }
    }
}
