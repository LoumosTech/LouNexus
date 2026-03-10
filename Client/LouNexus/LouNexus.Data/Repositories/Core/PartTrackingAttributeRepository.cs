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
    public class PartTrackingAttributeRepository : IPartTrackingAttributeRepository
    {
        // define private field for IDbConnectionProvider
        private readonly IDbConnectionProvider _connectionProvider;

        public PartTrackingAttributeRepository(IDbConnectionProvider connectionProvider)
        {

            // assign the connection provider.
            _connectionProvider = connectionProvider;
        }

        // Implement the GetAllAsync method to retrieve all part tracking attributes from the database.
        public async Task<IEnumerable<PartTrackingAttribute>> GetAllAsync()
        {
            // Create a list to hold the part tracking attributes.
            var partTrackingAttributes = new List<PartTrackingAttribute>();

            // Use the connection provider to create a connection to the database.
            using var connection = _connectionProvider.CreateConnection();

            // Check if the connection is a DbConnection, if not throw an exception.
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The connection provider did not return a valid DbConnection.");
            }

            // Open the database connection.
            await dbConnection.OpenAsync();

            // Create a command to execute the SQL query to retrieve all part tracking attributes.
            using var command = dbConnection.CreateCommand();

            // Set the command text to select all part tracking attributes from the core.part_tracking_attribute table.
            command.CommandText = @"
                SELECT 
                    part_tracking_attribute_id, 
                    part_id, 
                    workstation_type_id, 
                    attribute_name, 
                    is_required, 
                    sort_order, 
                    is_active, 
                    created_utc
                FROM core.part_tracking_attribute";

            // Execute the command and get a data reader to read the results.
            using var reader = await command.ExecuteReaderAsync();

            // Read each record from the data reader and create a PartTrackingAttribute object for each record, then add it to the list.
            while (await reader.ReadAsync())
            {
                var partTrackingAttribute = new PartTrackingAttribute
                {
                    PartTrackingAttributeId = reader.GetInt32(reader.GetOrdinal("part_tracking_attribute_id")),
                    PartId = reader.GetInt32(reader.GetOrdinal("part_id")),
                    WorkStationTypeId = reader.GetInt32(reader.GetOrdinal("workstation_type_id")),
                    AttributeName = reader.GetString(reader.GetOrdinal("attribute_name")),
                    IsRequired = reader.GetBoolean(reader.GetOrdinal("is_required")),
                    SortOrder = reader.GetInt32(reader.GetOrdinal("sort_order")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                };
                partTrackingAttributes.Add(partTrackingAttribute);
            }

            // retrn the list of part tracking attributes.
            return partTrackingAttributes;

        }

        // Implement the GetByIdAsync method to retrieve a specific part tracking attribute by its ID from the database.
        public async Task<PartTrackingAttribute?> GetByIdAsync(int id)
        {
            // Use the connection provider to create a connection to the database.
            using var connection = _connectionProvider.CreateConnection();

            // Check if the connection is a DbConnection, if not throw an exception.
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The connection provider did not return a valid DbConnection.");
            }

            // Open the database connection.
            await dbConnection.OpenAsync();

            // Create a command to execute the SQL query to retrieve the part tracking attribute with the specified ID.
            using var command = dbConnection.CreateCommand();

            // Set the command text to select the part tracking attribute from the core.part_tracking_attribute table where the part_tracking_attribute_id matches the specified ID.
            command.CommandText = @"
                SELECT 
                    part_tracking_attribute_id, 
                    part_id, 
                    workstation_type_id, 
                    attribute_name, 
                    is_required, 
                    sort_order, 
                    is_active, 
                    created_utc
                FROM core.part_tracking_attribute
                WHERE part_tracking_attribute_id = @id";

            // Create a parameter for the ID and add it to the command.
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            // Execute the command and get a data reader to read the result.
            using var reader = await command.ExecuteReaderAsync();

            // Read the record from the data reader and create a PartTrackingAttribute object if a record is found, then return it. If no record is found, return null.
            if (await reader.ReadAsync())
            {
                return new PartTrackingAttribute
                {
                    PartTrackingAttributeId = reader.GetInt32(reader.GetOrdinal("part_tracking_attribute_id")),
                    PartId = reader.GetInt32(reader.GetOrdinal("part_id")),
                    WorkStationTypeId = reader.GetInt32(reader.GetOrdinal("workstation_type_id")),
                    AttributeName = reader.GetString(reader.GetOrdinal("attribute_name")),
                    IsRequired = reader.GetBoolean(reader.GetOrdinal("is_required")),
                    SortOrder = reader.GetInt32(reader.GetOrdinal("sort_order")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                };
            }

            // If no record is found, return null.
            return null;
        }

        // Implement the InsertAsync method to insert a new part tracking attribute into the database and return the ID of the newly inserted record.
        public async Task<int> InsertAsync(PartTrackingAttribute partTrackingAttribute)
        {
            // Use the connection provider to create a connection to the database.
            using var connection = _connectionProvider.CreateConnection();

            // Check if the connection is a DbConnection, if not throw an exception.
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The connection provider did not return a valid DbConnection.");
            }

            // Open the database connection.
            await dbConnection.OpenAsync();

            // Create a command to execute the SQL query to insert a new part tracking attribute into the core.part_tracking_attribute table and return the ID of the newly inserted record.
            using var command = dbConnection.CreateCommand();

            // Set the command text to insert a new part tracking attribute into the core.part_tracking_attribute table and return the ID of the newly inserted record.
            command.CommandText = @"
                INSERT INTO core.part_tracking_attribute 
                    (part_id, workstation_type_id, attribute_name, is_required, sort_order, is_active, created_utc)
                VALUES 
                    (@partId, @workStationTypeId, @attributeName, @isRequired, @sortOrder, @isActive, @createdUtc)
                RETURNING part_tracking_attribute_id";

            // Create parameters for each property of the PartTrackingAttribute object and add them to the command.
            var partIdParameter = command.CreateParameter();
            partIdParameter.ParameterName = "@partId";
            partIdParameter.Value = partTrackingAttribute.PartId;
            command.Parameters.Add(partIdParameter);

            var workStationTypeIdParameter = command.CreateParameter();
            workStationTypeIdParameter.ParameterName = "@workStationTypeId";
            workStationTypeIdParameter.Value = partTrackingAttribute.WorkStationTypeId;
            command.Parameters.Add(workStationTypeIdParameter);

            var attributeNameParameter = command.CreateParameter();
            attributeNameParameter.ParameterName = "@attributeName";
            attributeNameParameter.Value = partTrackingAttribute.AttributeName;
            command.Parameters.Add(attributeNameParameter);

            var isRequiredParameter = command.CreateParameter();
            isRequiredParameter.ParameterName = "@isRequired";
            isRequiredParameter.Value = partTrackingAttribute.IsRequired;
            command.Parameters.Add(isRequiredParameter);

            var sortOrderParameter = command.CreateParameter();
            sortOrderParameter.ParameterName = "@sortOrder";
            sortOrderParameter.Value = partTrackingAttribute.SortOrder;
            command.Parameters.Add(sortOrderParameter);

            var isActiveParameter = command.CreateParameter();
            isActiveParameter.ParameterName = "@isActive";
            isActiveParameter.Value = partTrackingAttribute.IsActive;
            command.Parameters.Add(isActiveParameter);

            var createdUtcParameter = command.CreateParameter();
            createdUtcParameter.ParameterName = "@createdUtc";
            createdUtcParameter.Value = partTrackingAttribute.CreatedUtc;
            command.Parameters.Add(createdUtcParameter);

            // Execute the command and get the result, which should be the ID of the newly inserted record.
            object? result = await command.ExecuteScalarAsync();

            // Check if the result is null or DBNull, if so throw an exception. Otherwise, convert the result to an integer and return it.
            if (result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException("Failed to insert the part tracking attribute and retrieve the new ID.");
            }

            // Convert the result to an integer and return it.
            return Convert.ToInt32(result);
        }

        // Implement the UpdateAsync method to update an existing part tracking attribute in the database and return a boolean indicating whether the update was successful.
        public async Task<bool> UpdateAsync(PartTrackingAttribute partTrackingAttribute)
        {
            // Use the connection provider to create a connection to the database.
            using var connection = _connectionProvider.CreateConnection();

            // Check if the connection is a DbConnection, if not throw an exception.
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The connection provider did not return a valid DbConnection.");
            }

            // Open the database connection.
            await dbConnection.OpenAsync();

            // Create a command to execute the SQL query to update the existing part tracking attribute in the core.part_tracking_attribute table where the part_tracking_attribute_id matches the ID of the provided PartTrackingAttribute object.
            using var command = dbConnection.CreateCommand();

            // Set the command text to update the existing part tracking attribute in the core.part_tracking_attribute table where the part_tracking_attribute_id matches the ID of the provided PartTrackingAttribute object.
            command.CommandText = @"
                UPDATE core.part_tracking_attribute
                SET 
                    part_id = @partId,
                    workstation_type_id = @workStationTypeId,
                    attribute_name = @attributeName,
                    is_required = @isRequired,
                    sort_order = @sortOrder,
                    is_active = @isActive
                WHERE part_tracking_attribute_id = @id";

            // Create parameters for each property of the PartTrackingAttribute object and add them to the command, including a parameter for the ID to identify which record to update.
            var partIdParameter = command.CreateParameter();
            partIdParameter.ParameterName = "@partId";
            partIdParameter.Value = partTrackingAttribute.PartId;
            command.Parameters.Add(partIdParameter);

            var workStationTypeIdParameter = command.CreateParameter();
            workStationTypeIdParameter.ParameterName = "@workStationTypeId";
            workStationTypeIdParameter.Value = partTrackingAttribute.WorkStationTypeId;
            command.Parameters.Add(workStationTypeIdParameter);

            var attributeNameParameter = command.CreateParameter();
            attributeNameParameter.ParameterName = "@attributeName";
            attributeNameParameter.Value = partTrackingAttribute.AttributeName;
            command.Parameters.Add(attributeNameParameter);

            var isRequiredParameter = command.CreateParameter();
            isRequiredParameter.ParameterName = "@isRequired";
            isRequiredParameter.Value = partTrackingAttribute.IsRequired;
            command.Parameters.Add(isRequiredParameter);

            var sortOrderParameter = command.CreateParameter();
            sortOrderParameter.ParameterName = "@sortOrder";
            sortOrderParameter.Value = partTrackingAttribute.SortOrder;
            command.Parameters.Add(sortOrderParameter);

            var isActiveParameter = command.CreateParameter();
            isActiveParameter.ParameterName = "@isActive";
            isActiveParameter.Value = partTrackingAttribute.IsActive;
            command.Parameters.Add(isActiveParameter);

            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = partTrackingAttribute.PartTrackingAttributeId;
            command.Parameters.Add(idParameter);

            // Execute the command and get the number of rows affected by the update operation.
            int rowsAffected = await command.ExecuteNonQueryAsync();

            // Return true if at least one row was affected (indicating a successful update), otherwise return false.
            return rowsAffected > 0;

        }

        // Implement the DeleteAsync method to delete a part tracking attribute from the database by its ID and return a boolean indicating whether the deletion was successful.
        public async Task<bool> DeleteAsync(int id)
        {
            // Use the connection provider to create a connection to the database.
            using var connection = _connectionProvider.CreateConnection();

            // Check if the connection is a DbConnection, if not throw an exception.
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The connection provider did not return a valid DbConnection.");
            }

            // Open the database connection.
            await dbConnection.OpenAsync();

            // Create a command to execute the SQL query to delete the part tracking attribute from the core.part_tracking_attribute table where the part_tracking_attribute_id matches the specified ID.
            using var command = dbConnection.CreateCommand();

            // Set the command text to delete the part tracking attribute from the core.part_tracking_attribute table where the part_tracking_attribute_id matches the specified ID.
            command.CommandText = @"
                DELETE FROM core.part_tracking_attribute
                WHERE part_tracking_attribute_id = @id";

            // Create a parameter for the ID and add it to the command.
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            // Execute the command and get the number of rows affected by the delete operation.
            int rowsAffected = await command.ExecuteNonQueryAsync();

            // Return true if at least one row was affected (indicating a successful deletion), otherwise return false.
            return rowsAffected > 0;
        }
    }
}
