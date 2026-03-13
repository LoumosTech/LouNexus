using LouNexus.Core.Interfaces.Prod;
using LouNexus.Core.Models.Core;
using LouNexus.Core.Models.Prod;
using LouNexus.Data.DataBase;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Data.Repositories.Prod
{
    public class InspectionRepository : IInspectionRepository
    {
        // define the connection provider
        private readonly IDbConnectionProvider _connectionProvider;

        public InspectionRepository(IDbConnectionProvider connectionProvider)
        {
            // assign the connection provider
            _connectionProvider = connectionProvider;
        }

        // implement the GetAllAsync method to retrieve all inspections from the database
        public async Task<IEnumerable<Inspection>> GetAllAsync()
        {
            // create a list to hold the inspections
            var inspections = new List<Inspection>();

            // create a database connection using the connection provider
            using var connection = _connectionProvider.CreateConnection();

            // ensure the connection is a DbConnection to use ADO.NET features
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query
            using var command = dbConnection.CreateCommand();

            // set the command text to select all inspections from the prod.inspection table
            command.CommandText = @"
                SELECT 
                    inspection_id, 
                    inspector_number, 
                    factory_id, 
                    part_id, 
                    initial_quality, 
                    status, 
                    notes, 
                    created_utc
                FROM prod.inspection";

            // execute the command and get a data reader
            using var reader = await command.ExecuteReaderAsync();

            // read each record from the data reader and create an Inspection object to add to the list
            while (await reader.ReadAsync())
            {
                inspections.Add(new Inspection
                {
                    InspectionId = reader.GetInt32(reader.GetOrdinal("inspection_id")),
                    InspectorNumber = reader.GetString(reader.GetOrdinal("inspector_number")),
                    FactoryId = reader.GetInt32(reader.GetOrdinal("factory_id")),
                    PartId = reader.GetInt32(reader.GetOrdinal("part_id")),
                    InitialQuality = reader.GetInt32(reader.GetOrdinal("initial_quality")),
                    Status = reader.GetString(reader.GetOrdinal("status")),
                    Notes = reader.GetString(reader.GetOrdinal("notes"))
                });
            }

            // return the list of inspections
            return inspections;
        }

        // implement the GetByIdAsync method to retrieve a specific inspection by its ID from the database
        public async Task<Inspection?> GetByIdAsync(int id)
        {
            // create a database connection using the connection provider
            using var connection = _connectionProvider.CreateConnection();

            // ensure the connection is a DbConnection to use ADO.NET features
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query
            using var command = dbConnection.CreateCommand();

            // set the command text to select the inspection with the specified ID from the prod.inspection table
            command.CommandText = @"
                SELECT 
                    inspection_id, 
                    inspector_number, 
                    factory_id, 
                    part_id, 
                    initial_quality, 
                    status, 
                    notes, 
                    created_utc
                FROM prod.inspection
                WHERE inspection_id = @id";

            // create a parameter for the ID and add it to the command
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            // execute the command and get a data reader
            using var reader = await command.ExecuteReaderAsync();

            // read the record from the data reader and create an Inspection object to return
            if (await reader.ReadAsync())
            {
                return new Inspection
                {
                    InspectionId = reader.GetInt32(reader.GetOrdinal("inspection_id")),
                    InspectorNumber = reader.GetString(reader.GetOrdinal("inspector_number")),
                    FactoryId = reader.GetInt32(reader.GetOrdinal("factory_id")),
                    PartId = reader.GetInt32(reader.GetOrdinal("part_id")),
                    InitialQuality = reader.GetInt32(reader.GetOrdinal("initial_quality")),
                    Status = reader.GetString(reader.GetOrdinal("status")),
                    Notes = reader.GetString(reader.GetOrdinal("notes")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                };
            }

            // return null if no record was found
            return null;
        }

        // implement the InsertAsync method to add a new inspection to the database and return the new inspection ID
        public async Task<int> InsertAsync(Inspection inspection)
        {
            // create a database connection using the connection provider
            using var connection = _connectionProvider.CreateConnection();

            // ensure the connection is a DbConnection to use ADO.NET features
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query
            using var command = dbConnection.CreateCommand();

            // set the command text to insert a new inspection into the prod.inspection table and return the new inspection ID
            command.CommandText = @"
                INSERT INTO prod.inspection (
                    inspector_number, 
                    factory_id, 
                    part_id, 
                    initial_quality, 
                    status, 
                    notes, 
                    created_utc
                ) VALUES (
                    @inspector_number, 
                    @factory_id, 
                    @part_id, 
                    @initial_quality, 
                    @status, 
                    @notes, 
                    @created_utc
                ) RETURNING inspection_id;";

            // create parameters for each property of the inspection and add them to the command
            var inspectorNumberParameter = command.CreateParameter();
            inspectorNumberParameter.ParameterName = "@inspector_number";
            inspectorNumberParameter.Value = inspection.InspectorNumber;
            command.Parameters.Add(inspectorNumberParameter);

            var factoryIdParameter = command.CreateParameter();
            factoryIdParameter.ParameterName = "@factory_id";
            factoryIdParameter.Value = inspection.FactoryId;
            command.Parameters.Add(factoryIdParameter);

            var partIdParameter = command.CreateParameter();
            partIdParameter.ParameterName = "@part_id";
            partIdParameter.Value = inspection.PartId;
            command.Parameters.Add(partIdParameter);

            var initialQualityParameter = command.CreateParameter();
            initialQualityParameter.ParameterName = "@initial_quality";
            initialQualityParameter.Value = inspection.InitialQuality;
            command.Parameters.Add(initialQualityParameter);

            var statusParameter = command.CreateParameter();
            statusParameter.ParameterName = "@status";
            statusParameter.Value = inspection.Status;
            command.Parameters.Add(statusParameter);

            var notesParameter = command.CreateParameter();
            notesParameter.ParameterName = "@notes";
            notesParameter.Value = inspection.Notes;
            command.Parameters.Add(notesParameter);

            var createdUtcParameter = command.CreateParameter();
            createdUtcParameter.ParameterName = "@created_utc";
            createdUtcParameter.Value = inspection.CreatedUtc;
            command.Parameters.Add(createdUtcParameter);

            var clockedUtcParameter = command.CreateParameter();
            clockedUtcParameter.ParameterName = "@clocked_utc";
            clockedUtcParameter.Value = (object?)inspection.CreatedUtc ?? DBNull.Value;
            command.Parameters.Add(clockedUtcParameter);

            // execute the command and get the new inspection ID
            object? result = await command.ExecuteScalarAsync();

            // check if the result is null or DBNull and throw an exception if it is
            if (result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException("Failed to insert the inspection and retrieve the new ID.");
            }

            // return the new inspection ID as an integer
            return Convert.ToInt32(result);
        }

        // implement the UpdateAsync method to update an existing inspection in the database and return a boolean indicating success
        public async Task<bool> UpdateAsync(Inspection inspection)
        {
            // create a database connection using the connection provider
            var connection = _connectionProvider.CreateConnection();

            // ensure the connection is a DbConnection to use ADO.NET features
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query
            using var command = dbConnection.CreateCommand();

            // set the command text to update the existing inspection in the prod.inspection table with the specified ID
            command.CommandText = @"
                UPDATE prod.inspection
                SET 
                    inspector_number = @inspector_number, 
                    factory_id = @factory_id,
                    part_id = @part_id,
                    initial_quantity = @initial_quantity,
                    status = @status,
                    notes = @notes
                WHERE inspection_id = @id";

            // create parameters for each property of the inspection and add them to the command
            var inspectorNumberParameter = command.CreateParameter();
            inspectorNumberParameter.ParameterName = "@inspector_number";
            inspectorNumberParameter.Value = inspection.InspectorNumber;
            command.Parameters.Add(inspectorNumberParameter);

            var factoryIdParameter = command.CreateParameter();
            factoryIdParameter.ParameterName = "@factory_id";
            factoryIdParameter.Value = inspection.FactoryId;
            command.Parameters.Add(factoryIdParameter);

            var partIdParameter = command.CreateParameter();
            partIdParameter.ParameterName = "@part_id";
            partIdParameter.Value = inspection.PartId;
            command.Parameters.Add(partIdParameter);

            var initialQualityParameter = command.CreateParameter();
            initialQualityParameter.ParameterName = "@initial_quality";
            initialQualityParameter.Value = inspection.InitialQuality;
            command.Parameters.Add(initialQualityParameter);

            var statusParameter = command.CreateParameter();
            statusParameter.ParameterName = "@status";
            statusParameter.Value = inspection.Status;
            command.Parameters.Add(statusParameter);

            var notesParameter = command.CreateParameter();
            notesParameter.ParameterName = "@notes";
            notesParameter.Value = inspection.Notes;
            command.Parameters.Add(notesParameter);

            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = inspection.InspectionId;
            command.Parameters.Add(idParameter);

            // execute the command and get the number of rows affected
            int rowsAffected = await command.ExecuteNonQueryAsync();

            // return true if at least one row was affected, indicating the update was successful
            return rowsAffected > 0;
        }

        // implement the DeleteAsync method to remove an inspection from the database by its ID and return a boolean indicating success
        public async Task<bool> DeleteAsync(int id)
        {
            // create a database connection using the connection provider
            using var connection = _connectionProvider.CreateConnection();

            // ensure the connection is a DbConnection to use ADO.NET features
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query
            using var command = dbConnection.CreateCommand();

            // set the command text to delete the inspection with the specified ID from the prod.inspection table
            command.CommandText = @"
                DELETE FROM prod.inspection
                WHERE inspection_id = @id";

            // create a parameter for the ID and add it to the command
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            // execute the command and get the number of rows affected
            int rowsAffected = await command.ExecuteNonQueryAsync();

            // return true if at least one row was affected, indicating the delete was successful
            return rowsAffected > 0;
        }
    }
}
