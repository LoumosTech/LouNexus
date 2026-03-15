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
    public class StationEventRepository : IStationEventRepository
    {
        // define db connection provider.
        private readonly IDbConnectionProvider _connectionProvider;

        public StationEventRepository(IDbConnectionProvider connectionProvider)
        {
            // assign db connection provider.
            _connectionProvider = connectionProvider;
        }

        // implement the GetAllAsync method to retrieve all station events from the database.
        public async Task<IEnumerable<StationEvent>> GetAllAsync()
        {
            // create a list to hold the station events.
            var stationEvents = new List<StationEvent>();

            // create a database connection using the connection provider.
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection and throw an exception if not.
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection asynchronously.
            await dbConnection.OpenAsync();

            // create a database command to select all station events from the prod.station_event table.
            using var command = dbConnection.CreateCommand();

            // set the command text to select all station events.
            command.CommandText = @"
                SELECT station_event_id, 
                    inspection_id, 
                    work_station_id, 
                    event_type, 
                    good_quantity, 
                    notes, 
                    event_time_utc, 
                    created_utc
                FROM prod.station_event;";

            // execute the command and get a data reader to read the results.
            using var reader = await command.ExecuteReaderAsync();

            // read each record from the data reader and create a StationEvent object for each record, then add it to the list.
            while (await reader.ReadAsync())
            {
                stationEvents.Add(new StationEvent
                {
                    StationEventId = reader.GetInt32(reader.GetOrdinal("station_event_id")),
                    InspectionId = reader.GetInt32(reader.GetOrdinal("inspection_id")),
                    WorkStationId = reader.GetInt32(reader.GetOrdinal("work_station_id")),
                    EventType = reader.GetString(reader.GetOrdinal("event_type")),
                    GoodQuantity = reader.GetInt32(reader.GetOrdinal("good_quantity")),
                    Notes = reader.GetString(reader.GetOrdinal("notes")),
                    EventTimeUtc = reader.IsDBNull(reader.GetOrdinal("event_time_utc"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("event_time_utc")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                });
            }

            // return the list of station events.
            return stationEvents;
        }

        // implement the GetByIdAsync method to retrieve a station event by its ID from the database.
        public async Task<StationEvent?> GetByIdAsync(int id)
        {
            // create a database connection using the connection provider.
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection and throw an exception if not.
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection asynchronously.
            await dbConnection.OpenAsync();

            // create a database command to select a station event by its ID from the prod.station_event table.
            using var command = dbConnection.CreateCommand();

            // set the command text to select a station event by its ID.
            command.CommandText = @"
                SELECT 
                    station_event_id, 
                    inspection_id, 
                    work_station_id, 
                    event_type, 
                    good_quantity, 
                    notes, 
                    event_time_utc, 
                    created_utc
                FROM prod.station_event
                WHERE station_event_id = @Id;";

            // create a parameter for the ID and add it to the command.
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@Id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            // execute the command and get a data reader to read the result.
            using var reader = await command.ExecuteReaderAsync();

            // read the record from the data reader and create a StationEvent object if a record is found, then return it. If no record is found, return null.
            if (await reader.ReadAsync())
            {
                return new StationEvent
                {
                    StationEventId = reader.GetInt32(reader.GetOrdinal("station_event_id")),
                    InspectionId = reader.GetInt32(reader.GetOrdinal("inspection_id")),
                    WorkStationId = reader.GetInt32(reader.GetOrdinal("work_station_id")),
                    EventType = reader.GetString(reader.GetOrdinal("event_type")),
                    GoodQuantity = reader.GetInt32(reader.GetOrdinal("good_quantity")),
                    Notes = reader.GetString(reader.GetOrdinal("notes")),
                    EventTimeUtc = reader.IsDBNull(reader.GetOrdinal("event_time_utc"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("event_time_utc")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                };
            }

            // if no record is found, return null.
            return null;
        }

        // implement the InsertAsync method to insert a new station event into the database and return the new record's ID.
        public async Task<int> InsertAsync(StationEvent stationEvent)
        {
            // create a database connection using the connection provider.
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection and throw an exception if not.
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection asynchronously.
            await dbConnection.OpenAsync();

            // create a database command to insert a new station event into the prod.station_event table and return the new record's ID.
            using var command = dbConnection.CreateCommand();

            // set the command text to insert a new station event and return the new record's ID.
            command.CommandText = @"
                INSERT INTO prod.station_event (
                    inspection_id, 
                    work_station_id, 
                    event_type, 
                    good_quantity, 
                    notes, 
                    event_time_utc, 
                    created_utc
                ) VALUES (
                    @InspectionId, 
                    @WorkStationId, 
                    @EventType, 
                    @GoodQuantity, 
                    @Notes, 
                    @EventTimeUtc, 
                    @CreatedUtc
                )
                RETURNING station_event_id;";

            // create parameters for the station event properties and add them to the command.
            var inspectionIdParameter = command.CreateParameter();
            inspectionIdParameter.ParameterName = "@InspectionId";
            inspectionIdParameter.Value = stationEvent.InspectionId;
            command.Parameters.Add(inspectionIdParameter);

            var workStationIdParameter = command.CreateParameter();
            workStationIdParameter.ParameterName = "@WorkStationId";
            workStationIdParameter.Value = stationEvent.WorkStationId;
            command.Parameters.Add(workStationIdParameter);

            var eventTypeParameter = command.CreateParameter();
            eventTypeParameter.ParameterName = "@EventType";
            eventTypeParameter.Value = stationEvent.EventType;
            command.Parameters.Add(eventTypeParameter);

            var goodQuantityParameter = command.CreateParameter();
            goodQuantityParameter.ParameterName = "@GoodQuantity";
            goodQuantityParameter.Value = stationEvent.GoodQuantity;
            command.Parameters.Add(goodQuantityParameter);

            var notesParameter = command.CreateParameter();
            notesParameter.ParameterName = "@Notes";
            notesParameter.Value = stationEvent.Notes;
            command.Parameters.Add(notesParameter);

            var eventTimeUtcParameter = command.CreateParameter();
            eventTimeUtcParameter.ParameterName = "@EventTimeUtc";
            eventTimeUtcParameter.Value = stationEvent.EventTimeUtc.HasValue ? stationEvent.EventTimeUtc.Value : DBNull.Value;
            command.Parameters.Add(eventTimeUtcParameter);

            var createdUtcParameter = command.CreateParameter();
            createdUtcParameter.ParameterName = "@CreatedUtc";
            createdUtcParameter.Value = stationEvent.CreatedUtc;
            command.Parameters.Add(createdUtcParameter);

            // execute the command and get the new record's ID from the result.
            object? result = await command.ExecuteScalarAsync();

            // return the new record's ID as an integer. If the result is null, throw an exception.
            return Convert.ToInt32(result);
        }

        // implement the UpdateAsync method to update an existing station event in the database and return a boolean indicating success.
        public async Task<bool> UpdateAsync(StationEvent stationEvent)
        {
            // create a database connection using the connection provider.
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection and throw an exception if not.
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection asynchronously.
            await dbConnection.OpenAsync();

            // create a database command to update an existing station event in the prod.station_event table and return a boolean indicating success.
            using var command = dbConnection.CreateCommand();

            // set the command text to update an existing station event and return a boolean indicating success.
            command.CommandText = @"
                UPDATE prod.station_event
                SET 
                    inspection_id = @InspectionId,
                    work_station_id = @WorkStationId,
                    event_type = @EventType,
                    good_quantity = @GoodQuantity,
                    notes = @Notes,
                    event_time_utc = @EventTimeUtc,
                    created_utc = @CreatedUtc
                WHERE station_event_id = @StationEventId;";

            // create parameters for the station event properties and add them to the command.
            var stationEventIdParameter = command.CreateParameter();
            stationEventIdParameter.ParameterName = "@StationEventId";
            stationEventIdParameter.Value = stationEvent.StationEventId;
            command.Parameters.Add(stationEventIdParameter);

            var inspectionIdParameter = command.CreateParameter();
            inspectionIdParameter.ParameterName = "@InspectionId";
            inspectionIdParameter.Value = stationEvent.InspectionId;
            command.Parameters.Add(inspectionIdParameter);

            var workStationIdParameter = command.CreateParameter();
            workStationIdParameter.ParameterName = "@WorkStationId";
            workStationIdParameter.Value = stationEvent.WorkStationId;
            command.Parameters.Add(workStationIdParameter);

            var eventTypeParameter = command.CreateParameter();
            eventTypeParameter.ParameterName = "@EventType";
            eventTypeParameter.Value = stationEvent.EventType;
            command.Parameters.Add(eventTypeParameter);

            var goodQuantityParameter = command.CreateParameter();
            goodQuantityParameter.ParameterName = "@GoodQuantity";
            goodQuantityParameter.Value = stationEvent.GoodQuantity;
            command.Parameters.Add(goodQuantityParameter);

            var notesParameter = command.CreateParameter();
            notesParameter.ParameterName = "@Notes";
            notesParameter.Value = stationEvent.Notes;
            command.Parameters.Add(notesParameter);

            var eventTimeUtcParameter = command.CreateParameter();
            eventTimeUtcParameter.ParameterName = "@EventTimeUtc";
            eventTimeUtcParameter.Value = stationEvent.EventTimeUtc.HasValue ? stationEvent.EventTimeUtc.Value : DBNull.Value;
            command.Parameters.Add(eventTimeUtcParameter);

            var createdUtcParameter = command.CreateParameter();
            createdUtcParameter.ParameterName = "@CreatedUtc";
            createdUtcParameter.Value = stationEvent.CreatedUtc;
            command.Parameters.Add(createdUtcParameter);

            // execute the command and get the number of rows affected by the update.
            int rowsAffected = await command.ExecuteNonQueryAsync();

            // return true if at least one row was affected, indicating that the update was successful; otherwise, return false.
            return rowsAffected > 0;
        }

        // implement the DeleteAsync method to delete a station event by its ID from the database and return a boolean indicating success.
        public async Task<bool> DeleteAsync(int id)
        {
            // create a database connection using the connection provider.
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection and throw an exception if not.
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection asynchronously.
            await dbConnection.OpenAsync();

            // create a database command to delete a station event by its ID from the prod.station_event table and return a boolean indicating success.
            using var command = dbConnection.CreateCommand();

            // set the command text to delete a station event by its ID and return a boolean indicating success.
            command.CommandText = @"
                DELETE FROM prod.station_event
                WHERE station_event_id = @Id;";

            // create a parameter for the ID and add it to the command.
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@Id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            // execute the command and get the number of rows affected by the delete.
            int rowsAffected = await command.ExecuteNonQueryAsync();

            // return true if at least one row was affected, indicating that the delete was successful; otherwise, return false.
            return rowsAffected > 0;
        }
    }
}
