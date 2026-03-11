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
    public class PartWorkStationRequirementRepository : IPartWorkStationRequirementRepository
    {
        // define the connection provider
        private readonly IDbConnectionProvider _connectionProvider;

        public PartWorkStationRequirementRepository(IDbConnectionProvider connectionProvider)
        {
            //assign the connection provider
            _connectionProvider = connectionProvider;
        }

        // implement the GetAllAsync method to retrieve all PartWorkStationRequirement records from the database
        public async Task<IEnumerable<PartWorkStationRequirement>> GetAllAsync()
        {
            // create a list to hold the results
            var partWorkStationRequirements = new List<PartWorkStationRequirement>();

            // create a connection using the connection provider
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection, if not throw an exception
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query
            using var command = dbConnection.CreateCommand();

            // set the command text to select all records from the part_workStation_requirement table
            command.CommandText = @"
                SELECT part_workStation_requirement_id, part_id, workstation_type_id, sequence_order, is_required, created_utc
                FROM core.part_workStation_requirement";

            // execute the command and get a data reader
            using var reader = await command.ExecuteReaderAsync();

            // read each record from the data reader and add it to the list
            while (await reader.ReadAsync())
            {
                var partWorkStationRequirement = new PartWorkStationRequirement
                {
                    PartWorkStationRequirementId = reader.GetInt32(reader.GetOrdinal("part_workStation_requirement_id")),
                    PartId = reader.GetInt32(reader.GetOrdinal("part_id")),
                    WorkStationTypeId = reader.GetInt32(reader.GetOrdinal("workstation_type_id")),
                    SequenceOrder = reader.GetInt32(reader.GetOrdinal("sequence_order")),
                    IsRequired = reader.GetBoolean(reader.GetOrdinal("is_required")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                };
                partWorkStationRequirements.Add(partWorkStationRequirement);
            }

            // return the list of PartWorkStationRequirement records
            return partWorkStationRequirements;
        }

        // implement the GetByIdAsync method to retrieve a single PartWorkStationRequirement record by its ID from the database
        public async Task<PartWorkStationRequirement?> GetByIdAsync(int id)
        {
            // create a connection using the connection provider
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection, if not throw an exception
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query
            using var command = dbConnection.CreateCommand();

            // set the command text to select a record from the part_workStation_requirement table where the ID matches the provided ID
            command.CommandText = @"
                SELECT part_workStation_requirement_id, part_id, workstation_type_id, sequence_order, is_required, created_utc
                FROM core.part_workStation_requirement
                WHERE part_workStation_requirement_id = @id";

            // create a parameter for the ID and add it to the command
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            // execute the command and get a data reader
            using var reader = await command.ExecuteReaderAsync();

            // read the record from the data reader and return it as a PartWorkStationRequirement object, if found
            if (await reader.ReadAsync())
            {
                return new PartWorkStationRequirement
                {
                    PartWorkStationRequirementId = reader.GetInt32(reader.GetOrdinal("part_workStation_requirement_id")),
                    PartId = reader.GetInt32(reader.GetOrdinal("part_id")),
                    WorkStationTypeId = reader.GetInt32(reader.GetOrdinal("workstation_type_id")),
                    SequenceOrder = reader.GetInt32(reader.GetOrdinal("sequence_order")),
                    IsRequired = reader.GetBoolean(reader.GetOrdinal("is_required")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                };
            }

            // if no record is found, return null
            return null;
        }

        // implement the InsertAsync method to insert a new PartWorkStationRequirement record into the database and return the new record's ID
        public async Task<int> InsertAsync(PartWorkStationRequirement partWorkStationRequirement)
        {
            // create a connection using the connection provider
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection, if not throw an exception
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query
            using var command = dbConnection.CreateCommand();

            // set the command text to insert a new record into the part_workStation_requirement table and return the new record's ID
            command.CommandText = @"
                INSERT INTO core.part_workStation_requirement (part_id, workstation_type_id, sequence_order, is_required, created_utc)
                VALUES (@partId, @workStationTypeId, @sequenceOrder, @isRequired, @createdUtc)
                RETURNING part_workStation_requirement_id";

            // create parameters for the values to be inserted and add them to the command
            var partIdParameter = command.CreateParameter();
            partIdParameter.ParameterName = "@partId";
            partIdParameter.Value = partWorkStationRequirement.PartId;
            command.Parameters.Add(partIdParameter);

            var workStationTypeIdParameter = command.CreateParameter();
            workStationTypeIdParameter.ParameterName = "@workStationTypeId";
            workStationTypeIdParameter.Value = partWorkStationRequirement.WorkStationTypeId;
            command.Parameters.Add(workStationTypeIdParameter);

            var sequenceOrderParameter = command.CreateParameter();
            sequenceOrderParameter.ParameterName = "@sequenceOrder";
            sequenceOrderParameter.Value = partWorkStationRequirement.SequenceOrder;
            command.Parameters.Add(sequenceOrderParameter);

            var isRequiredParameter = command.CreateParameter();
            isRequiredParameter.ParameterName = "@isRequired";
            isRequiredParameter.Value = partWorkStationRequirement.IsRequired;
            command.Parameters.Add(isRequiredParameter);

            var createdUtcParameter = command.CreateParameter();
            createdUtcParameter.ParameterName = "@createdUtc";
            createdUtcParameter.Value = partWorkStationRequirement.CreatedUtc;
            command.Parameters.Add(createdUtcParameter);

            // execute the command and get the new record's ID
            object? result = await command.ExecuteScalarAsync();

            // check if the result is null or DBNull, if so throw an exception
            if (result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException("Failed to insert PartWorkStationRequirement and retrieve the new ID.");
            }

            // return the new record's ID as an integer
            return Convert.ToInt32(result);
        }

        // implement the UpdateAsync method to update an existing PartWorkStationRequirement record in the database and return a boolean indicating success
        public async Task<bool> UpdateAsync(PartWorkStationRequirement partWorkStationRequirement)
        {
            // create a connection using the connection provider
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection, if not throw an exception
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query
            using var command = dbConnection.CreateCommand();

            // set the command text to update an existing record in the part_workStation_requirement table where the ID matches the provided PartWorkStationRequirement's ID
            command.CommandText = @"
                UPDATE core.part_workStation_requirement
                SET part_id = @partId,
                    workstation_type_id = @workStationTypeId,
                    sequence_order = @sequenceOrder,
                    is_required = @isRequired,
                    created_utc = @createdUtc
                WHERE part_workStation_requirement_id = @id";

            // create parameters for the values to be updated and add them to the command
            var partIdParameter = command.CreateParameter();
            partIdParameter.ParameterName = "@partId";
            partIdParameter.Value = partWorkStationRequirement.PartId;
            command.Parameters.Add(partIdParameter);

            var workStationTypeIdParameter = command.CreateParameter();
            workStationTypeIdParameter.ParameterName = "@workStationTypeId";
            workStationTypeIdParameter.Value = partWorkStationRequirement.WorkStationTypeId;
            command.Parameters.Add(workStationTypeIdParameter);

            var sequenceOrderParameter = command.CreateParameter();
            sequenceOrderParameter.ParameterName = "@sequenceOrder";
            sequenceOrderParameter.Value = partWorkStationRequirement.SequenceOrder;
            command.Parameters.Add(sequenceOrderParameter);

            var isRequiredParameter = command.CreateParameter();
            isRequiredParameter.ParameterName = "@isRequired";
            isRequiredParameter.Value = partWorkStationRequirement.IsRequired;
            command.Parameters.Add(isRequiredParameter);

            var createdUtcParameter = command.CreateParameter();
            createdUtcParameter.ParameterName = "@createdUtc";
            createdUtcParameter.Value = partWorkStationRequirement.CreatedUtc;
            command.Parameters.Add(createdUtcParameter);

            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = partWorkStationRequirement.PartWorkStationRequirementId;
            command.Parameters.Add(idParameter);

            // execute the command and get the number of rows affected
            int rowsAffected = await command.ExecuteNonQueryAsync();

            // return true if at least one row was affected, indicating a successful update, otherwise return false
            return rowsAffected > 0;
        }

        // implement the DeleteAsync method to delete a PartWorkStationRequirement record from the database by its ID and return a boolean indicating success
        public async Task<bool> DeleteAsync(int id)
        {
            // create a connection using the connection provider
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection, if not throw an exception
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query
            using var command = dbConnection.CreateCommand();

            // set the command text to delete a record from the part_workStation_requirement table where the ID matches the provided ID
            command.CommandText = @"
                DELETE FROM core.part_workStation_requirement
                WHERE part_workStation_requirement_id = @id";

            // create a parameter for the ID and add it to the command
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            // execute the command and get the number of rows affected
            int rowsAffected = await command.ExecuteNonQueryAsync();

            // return true if at least one row was affected, indicating a successful delete, otherwise return false
            return rowsAffected > 0;
        }
    }
}
