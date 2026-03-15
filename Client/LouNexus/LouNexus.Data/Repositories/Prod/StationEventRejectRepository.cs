using LouNexus.Core.Interfaces.Prod;
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
    public class StationEventRejectRepository : IStationEventRejectRepository
    {
        // define the connection provider for database access
        private readonly IDbConnectionProvider _connectionProvider;

        public StationEventRejectRepository(IDbConnectionProvider connectionProvider)
        {
            // assign the db connection provider.
            _connectionProvider = connectionProvider;
        }

        // implement the GetAllAsync method to retrieve all station event rejects from the database
        public async Task<IEnumerable<StationEventReject>> GetAllAsync()
        {
            // create a list to hold the retrieved station event rejects
            var stationEventRejects = new List<StationEventReject>();

            // create and open a database connection using the connection provider
            using var connection = _connectionProvider.CreateConnection();

            // ensure the connection is a DbConnection to access ADO.NET features
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection asynchronously
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query to retrieve all station event rejects
            using var command = dbConnection.CreateCommand();

            // set the command text to select all relevant fields from the station_event_reject table
            command.CommandText = @"
                SELECT 
                    station_event_reject_id, 
                    station_event_id, 
                    reject_code_id, 
                    quantity, 
                    notes, 
                    created_utc
                FROM prod.station_event_reject;";

            // execute the command and obtain a data reader to read the results
            using var reader = await command.ExecuteReaderAsync();

            // read each record from the data reader and create a StationEventReject object for each record, adding it to the list
            while (await reader.ReadAsync())
            {
                stationEventRejects.Add(new StationEventReject
                {
                    StationEventRejectId = reader.GetInt32(reader.GetOrdinal("station_event_reject_id")),
                    StationEventId = reader.GetInt32(reader.GetOrdinal("station_event_id")),
                    RejectCodeId = reader.GetInt32(reader.GetOrdinal("reject_code_id")),
                    Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                    Notes = reader.GetString(reader.GetOrdinal("notes")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                });
            }

            // return the list of station event rejects
            return stationEventRejects;
        }

        // implement the GetByIdAsync method to retrieve a specific station event reject by its ID from the database
        public async Task<StationEventReject?> GetByIdAsync(int id)
        {
            // create and open a database connection using the connection provider
            using var connection = _connectionProvider.CreateConnection();

            // ensure the connection is a DbConnection to access ADO.NET features
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection asynchronously
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query to retrieve a station event reject by its ID
            using var command = dbConnection.CreateCommand();

            // set the command text to select the relevant fields from the station_event_reject table where the ID matches the provided parameter
            command.CommandText = @"
                SELECT 
                    station_event_reject_id, 
                    station_event_id, 
                    reject_code_id, 
                    quantity, 
                    notes, 
                    created_utc
                FROM prod.station_event_reject
                WHERE station_event_reject_id = @id;";

            // create a parameter for the ID and add it to the command
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.Value = id;
            command.Parameters.Add(parameter);

            // execute the command and obtain a data reader to read the result
            using var reader = await command.ExecuteReaderAsync();

            // read the record from the data reader and create a StationEventReject object if a record is found, otherwise return null
            if (await reader.ReadAsync())
            {
                return new StationEventReject
                {
                    StationEventRejectId = reader.GetInt32(reader.GetOrdinal("station_event_reject_id")),
                    StationEventId = reader.GetInt32(reader.GetOrdinal("station_event_id")),
                    RejectCodeId = reader.GetInt32(reader.GetOrdinal("reject_code_id")),
                    Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                    Notes = reader.GetString(reader.GetOrdinal("notes")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                };
            }

            // if no record is found, return null
            return null;
        }

        // implement the InsertAsync method to add a new station event reject to the database and return its new ID
        public async Task<int> InsertAsync(StationEventReject stationEventReject)
        {
            // create and open a database connection using the connection provider
            using var connection = _connectionProvider.CreateConnection();

            // ensure the connection is a DbConnection to access ADO.NET features
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection asynchronously
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query to insert a new station event reject and return the new ID
            using var command = dbConnection.CreateCommand();

            // set the command text to insert a new record into the station_event_reject table with the provided values and return the generated ID
            command.CommandText = @"
                INSERT INTO prod.station_event_reject 
                    (station_event_id, reject_code_id, quantity, notes, created_utc)
                VALUES 
                    (@stationEventId, @rejectCodeId, @quantity, @notes, @createdUtc)
                RETURNING station_event_reject_id;";

            // create parameters for each field and add them to the command
            var stationEventIdParam = command.CreateParameter();
            stationEventIdParam.ParameterName = "@stationEventId";
            stationEventIdParam.Value = stationEventReject.StationEventId;
            command.Parameters.Add(stationEventIdParam);

            var rejectCodeIdParam = command.CreateParameter();
            rejectCodeIdParam.ParameterName = "@rejectCodeId";
            rejectCodeIdParam.Value = stationEventReject.RejectCodeId;
            command.Parameters.Add(rejectCodeIdParam);

            var quantityParam = command.CreateParameter();
            quantityParam.ParameterName = "@quantity";
            quantityParam.Value = stationEventReject.Quantity;
            command.Parameters.Add(quantityParam);

            var notesParam = command.CreateParameter();
            notesParam.ParameterName = "@notes";
            notesParam.Value = stationEventReject.Notes;
            command.Parameters.Add(notesParam);

            var createdUtcParam = command.CreateParameter();
            createdUtcParam.ParameterName = "@createdUtc";
            createdUtcParam.Value = stationEventReject.CreatedUtc;
            command.Parameters.Add(createdUtcParam);

            // execute the command and retrieve the new ID of the inserted record
            object? result = await command.ExecuteScalarAsync();

            // convert the result to an integer and return it as the new ID of the inserted station event reject
            return Convert.ToInt32(result);
        }

        // implement the UpdateAsync method to update an existing station event reject in the database and return a boolean indicating success
        public async Task<bool> UpdateAsync(StationEventReject stationEventReject)
        {
            // create and open a database connection using the connection provider
            using var connection = _connectionProvider.CreateConnection();

            // ensure the connection is a DbConnection to access ADO.NET features
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection asynchronously
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query to update an existing station event reject with the provided values where the ID matches
            using var command = dbConnection.CreateCommand();

            // set the command text to update the relevant fields in the station_event_reject table where the ID matches the provided station event reject's ID
            command.CommandText = @"
                UPDATE prod.station_event_reject
                SET 
                    station_event_id = @stationEventId,
                    reject_code_id = @rejectCodeId,
                    quantity = @quantity,
                    notes = @notes,
                    created_utc = @createdUtc
                WHERE station_event_reject_id = @id;";

            // create parameters for each field and the ID, and add them to the command
            var idParam = command.CreateParameter();
            idParam.ParameterName = "@id";
            idParam.Value = stationEventReject.StationEventRejectId;
            command.Parameters.Add(idParam);

            var stationEventIdParam = command.CreateParameter();
            stationEventIdParam.ParameterName = "@stationEventId";
            stationEventIdParam.Value = stationEventReject.StationEventId;
            command.Parameters.Add(stationEventIdParam);

            var rejectCodeIdParam = command.CreateParameter();
            rejectCodeIdParam.ParameterName = "@rejectCodeId";
            rejectCodeIdParam.Value = stationEventReject.RejectCodeId;
            command.Parameters.Add(rejectCodeIdParam);

            var quantityParam = command.CreateParameter();
            quantityParam.ParameterName = "@quantity";
            quantityParam.Value = stationEventReject.Quantity;
            command.Parameters.Add(quantityParam);

            var notesParam = command.CreateParameter();
            notesParam.ParameterName = "@notes";
            notesParam.Value = stationEventReject.Notes;
            command.Parameters.Add(notesParam);

            var createdUtcParam = command.CreateParameter();
            createdUtcParam.ParameterName = "@createdUtc";
            createdUtcParam.Value = stationEventReject.CreatedUtc;
            command.Parameters.Add(createdUtcParam);

            // execute the command and retrieve the number of rows affected by the update operation
            var rowsAffected = await command.ExecuteNonQueryAsync();

            // return true if at least one row was affected (indicating a successful update), otherwise return false
            return rowsAffected > 0;
        }

        // implement the DeleteAsync method to remove a station event reject from the database by its ID and return a boolean indicating success
        public async Task<bool> DeleteAsync(int id)
        {
            // create and open a database connection using the connection provider
            using var connection = _connectionProvider.CreateConnection();

            // ensure the connection is a DbConnection to access ADO.NET features
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection asynchronously
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query to delete a station event reject by its ID
            using var command = dbConnection.CreateCommand();

            // set the command text to delete the record from the station_event_reject table where the ID matches the provided parameter
            command.CommandText = @"
                DELETE FROM prod.station_event_reject
                WHERE station_event_reject_id = @id;";

            // create a parameter for the ID and add it to the command
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.Value = id;
            command.Parameters.Add(parameter);

            // execute the command and retrieve the number of rows affected by the delete operation
            var rowsAffected = await command.ExecuteNonQueryAsync();

            // return true if at least one row was affected (indicating a successful delete), otherwise return false
            return rowsAffected > 0;
        }
    }
}
