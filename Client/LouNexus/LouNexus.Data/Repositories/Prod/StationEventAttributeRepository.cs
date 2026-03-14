using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LouNexus.Core.Interfaces.Prod;
using LouNexus.Core.Models.Prod;
using LouNexus.Data.DataBase;

namespace LouNexus.Data.Repositories.Prod
{
    public class StationEventAttributeRepository : IStationEventAttributeRepository
    {
        // define the connection provider.
        private readonly IDbConnectionProvider _connectionProvider;
        public StationEventAttributeRepository(IDbConnectionProvider connectionProvider)
        {
            // assign the connection provider.
            _connectionProvider = connectionProvider;
        }

        // implement the GetAllAsync method to retrieve all station event attributes from the database.
        public async Task<IEnumerable<StationEventAttribute>> GetAllAsync()
        {
            // create a list to hold the station event attributes.
            var StationEventAttributes = new List<StationEventAttribute>();

            // create a database connection using the connection provider.
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection, if not throw an exception.
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection.
            await dbConnection.OpenAsync();

            // create a database command to retrieve all station event attributes.
            using var command = dbConnection.CreateCommand();

            // set the command text to select all station event attributes from the database.
            command.CommandText = @"
                SELECT 
                    station_event_attribute_id, 
                    station_event_id, 
                    part_tracking_attribute_id, 
                    attribute_value, 
                    notes, 
                    created_utc
                FROM prod.station_event_attribute";

            // execute the command and read the results.
            using var reader = await command.ExecuteReaderAsync();

            // loop through the results and add each station event attribute to the list.
            while (await reader.ReadAsync())
            {
                StationEventAttributes.Add( new StationEventAttribute
                {
                    StationEventAttributeId = reader.GetInt32(reader.GetOrdinal("station_event_attribute_id")),
                    StationEventId = reader.GetInt32(reader.GetOrdinal("station_event_id")),
                    PartTrackingAttributeId = reader.GetInt32(reader.GetOrdinal("part_tracking_attribute_id")),
                    AttributeValue = reader.GetString(reader.GetOrdinal("attribute_value")),
                    Notes = reader.GetString(reader.GetOrdinal("notes")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                });
            }

            // return the list of station event attributes.
            return StationEventAttributes;
        }

        // implement the GetByIdAsync method to retrieve a station event attribute by its ID from the database.
        public async Task<StationEventAttribute?> GetByIdAsync(int id)
        {
            // create a database connection using the connection provider.
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection, if not throw an exception.
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection.
            await dbConnection.OpenAsync();

            // create a database command to retrieve a station event attribute by its ID.
            using var command = dbConnection.CreateCommand();

            // set the command text to select a station event attribute by its ID from the database.
            command.CommandText = @"
                SELECT 
                    station_event_attribute_id, 
                    station_event_id, 
                    part_tracking_attribute_id, 
                    attribute_value, 
                    notes, 
                    created_utc
                FROM prod.station_event_attribute
                WHERE station_event_attribute_id = @id";

            // create a parameter for the ID and add it to the command.
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            // execute the command and read the result.
            using var reader = await command.ExecuteReaderAsync();

            // if a station event attribute is found, return it, otherwise return null.
            if (await reader.ReadAsync())
            {
                return new StationEventAttribute
                {
                    StationEventAttributeId = reader.GetInt32(reader.GetOrdinal("station_event_attribute_id")),
                    StationEventId = reader.GetInt32(reader.GetOrdinal("station_event_id")),
                    PartTrackingAttributeId = reader.GetInt32(reader.GetOrdinal("part_tracking_attribute_id")),
                    AttributeValue = reader.GetString(reader.GetOrdinal("attribute_value")),
                    Notes = reader.GetString(reader.GetOrdinal("notes")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                };
            }

            // if no station event attribute is found, return null.
            return null;
        }

        // implement the InsertAsync method to insert a new station event attribute into the database and return the ID of the newly inserted record.
        public async Task<int> InsertAsync(StationEventAttribute stationEventAttribute)
        {
            // create a database connection using the connection provider.
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection, if not throw an exception.
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection.
            await dbConnection.OpenAsync();

            // create a database command to insert a new station event attribute into the database.
            using var command = dbConnection.CreateCommand();

            // set the command text to insert a new station event attribute into the database and return the ID of the newly inserted record.
            command.CommandText = @"
                INSERT INTO prod.station_event_attribute 
                    (station_event_id, part_tracking_attribute_id, attribute_value, notes, created_utc)
                VALUES 
                    (@stationEventId, @partTrackingAttributeId, @attributeValue, @notes, @createdUtc)
                RETURNING station_event_attribute_id;";

            // create parameters for the station event attribute properties and add them to the command.
            var stationEventIdParameter = command.CreateParameter();
            stationEventIdParameter.ParameterName = "@stationEventId";
            stationEventIdParameter.Value = stationEventAttribute.StationEventId;
            command.Parameters.Add(stationEventIdParameter);

            var partTrackingAttributeIdParameter = command.CreateParameter();
            partTrackingAttributeIdParameter.ParameterName = "@partTrackingAttributeId";
            partTrackingAttributeIdParameter.Value = stationEventAttribute.PartTrackingAttributeId;
            command.Parameters.Add(partTrackingAttributeIdParameter);

            var attributeValueParameter = command.CreateParameter();
            attributeValueParameter.ParameterName = "@attributeValue";
            attributeValueParameter.Value = stationEventAttribute.AttributeValue;
            command.Parameters.Add(attributeValueParameter);

            var notesParameter = command.CreateParameter();
            notesParameter.ParameterName = "@notes";
            notesParameter.Value = stationEventAttribute.Notes;
            command.Parameters.Add(notesParameter);

            var createdUtcParameter = command.CreateParameter();
            createdUtcParameter.ParameterName = "@createdUtc";
            createdUtcParameter.Value = stationEventAttribute.CreatedUtc;
            command.Parameters.Add(createdUtcParameter);

            // execute the command and retrieve the ID of the newly inserted record.
            object? result = await command.ExecuteScalarAsync();

            // check if the result is null or DBNull, if so throw an exception.
            if (result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException("Failed to insert the station event attribute and retrieve the new ID.");
            }

            // return the ID of the newly inserted record as an integer.
            return Convert.ToInt32(result);
        }

        // implement the UpdateAsync method to update an existing station event attribute in the database and return a boolean indicating whether the update was successful.
        public async Task<bool> UpdateAsync(StationEventAttribute stationEventAttribute)
        {
            // create a database connection using the connection provider.
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection, if not throw an exception.
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection.
            await dbConnection.OpenAsync();

            // create a database command to update an existing station event attribute in the database.
            using var command = dbConnection.CreateCommand();

            // set the command text to update an existing station event attribute in the database and return a boolean indicating whether the update was successful.
            command.CommandText = @"
                UPDATE prod.station_event_attribute 
                SET 
                    station_event_id = @stationEventId, 
                    part_tracking_attribute_id = @partTrackingAttributeId, 
                    attribute_value = @attributeValue, 
                    notes = @notes, 
                    created_utc = @createdUtc
                WHERE station_event_attribute_id = @id;";

            // create parameters for the station event attribute properties and add them to the command.
            var stationEventIdParameter = command.CreateParameter();
            stationEventIdParameter.ParameterName = "@stationEventId";
            stationEventIdParameter.Value = stationEventAttribute.StationEventId;
            command.Parameters.Add(stationEventIdParameter);

            var partTrackingAttributeIdParameter = command.CreateParameter();
            partTrackingAttributeIdParameter.ParameterName = "@partTrackingAttributeId";
            partTrackingAttributeIdParameter.Value = stationEventAttribute.PartTrackingAttributeId;
            command.Parameters.Add(partTrackingAttributeIdParameter);

            var attributeValueParameter = command.CreateParameter();
            attributeValueParameter.ParameterName = "@attributeValue";
            attributeValueParameter.Value = stationEventAttribute.AttributeValue;
            command.Parameters.Add(attributeValueParameter);    

            var notesParameter = command.CreateParameter();
            notesParameter.ParameterName = "@notes";
            notesParameter.Value = stationEventAttribute.Notes;
            command.Parameters.Add(notesParameter);

            var createdUtcParameter = command.CreateParameter();
            createdUtcParameter.ParameterName = "@createdUtc";
            createdUtcParameter.Value = stationEventAttribute.CreatedUtc;
            command.Parameters.Add(createdUtcParameter);

            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = stationEventAttribute.StationEventAttributeId;
            command.Parameters.Add(idParameter);

            // execute the command and retrieve the number of rows affected.
            int rowsAffected = await command.ExecuteNonQueryAsync();

            // return true if at least one row was affected, indicating that the update was successful, otherwise return false.
            return rowsAffected > 0;
        }

        // implement the DeleteAsync method to delete a station event attribute by its ID from the database and return a boolean indicating whether the deletion was successful.
        public async Task<bool> DeleteAsync(int id)
        {
            // create a database connection using the connection provider.
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection, if not throw an exception.
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection.
            await dbConnection.OpenAsync();

            // create a database command to delete a station event attribute by its ID from the database.
            using var command = dbConnection.CreateCommand();

            // set the command text to delete a station event attribute by its ID from the database and return a boolean indicating whether the deletion was successful.
            command.CommandText = @"
                DELETE FROM prod.station_event_attribute 
                WHERE station_event_attribute_id = @id;";

            // create a parameter for the ID and add it to the command.
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            // execute the command and retrieve the number of rows affected.
            int rowsAffected = await command.ExecuteNonQueryAsync();

            // return true if at least one row was affected, indicating that the deletion was successful, otherwise return false.
            return rowsAffected > 0;
        }
    }
}
