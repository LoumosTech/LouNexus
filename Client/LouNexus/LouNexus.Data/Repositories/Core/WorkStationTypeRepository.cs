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
    public class WorkStationTypeRepository : IWorkStationTypeRepository
    {
        // create db connection provider.
        private readonly IDbConnectionProvider _connectionProvider;


        public WorkStationTypeRepository(IDbConnectionProvider connectionProvider)
        {
            // assign database connection provider
            _connectionProvider = connectionProvider;
        }

        // implement the GetAllAsync method to retrieve all workstation types from the database.
        public async Task<IEnumerable<WorkStationType>> GetAllAsync()
        {
            // create a list to hold the workstation types retrieved from the database.
            var workstationTypes = new List<WorkStationType>();

            // create a database connection using the connection provider.
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection, if not throw an exception.
            if (connection is not DbConnection dbConnection)
            {
                throw new Exception("The database connection must inherit from DbConnection.");
            }

            // open the database connection.
            await dbConnection.OpenAsync();

            // create a database command to execute the SQL query to retrieve all workstation types.
            using var command = dbConnection.CreateCommand();

            // set the command text to select all workstation types from the core.workstation_type table.
            command.CommandText = @"
            SELECT
                workstation_type_id,
                workstation_type_name,
                workstation_type_code,
                supports_cpk,
                supports_reject_entry,
                supports_tracking_attributes,
                supports_clockout,
                is_active,
                created_utc
            FROM core.workstation_type";

            // execute the command and read the results using a data reader.
            using var reader = await command.ExecuteReaderAsync();

            // loop through the results and create WorkStationType objects to add to the list.
            while (reader.Read())
            {
                workstationTypes.Add(new WorkStationType
                {
                    WorkStationTypeId = reader.GetInt32(reader.GetOrdinal("workstation_type_id")),
                    WorkStationTypeName = reader.GetString(reader.GetOrdinal("workstation_type_name")),
                    WorkStationTypeCode = reader.GetString(reader.GetOrdinal("workstation_type_code")),
                    SupportsCpk = reader.GetBoolean(reader.GetOrdinal("supports_cpk")),
                    SupportsRejectEntry = reader.GetBoolean(reader.GetOrdinal("supports_reject_entry")),
                    SupportsTrackingAttributes = reader.GetBoolean(reader.GetOrdinal("supports_tracking_attributes")),
                    SupportsClockOut = reader.GetBoolean(reader.GetOrdinal("supports_clockout")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                });
            }

            // return the list of workstation types.
            return workstationTypes;
        }

        // implement the GetByIdAsync method to retrieve a workstation type from the database by ID.
        public async Task<WorkStationType?> GetByIdAsync(int id)
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

            // create a database command to execute the SQL query to retrieve a workstation type by ID.
            using var command = dbConnection.CreateCommand();

            // set the command text to select a workstation type from the core.workstation_type table where the workstation_type_id matches the provided ID.
            command.CommandText = @"
                SELECT
                    workstation_type_id,
                    workstation_type_name,
                    workstation_type_code,
                    supports_cpk,
                    supports_reject_entry,
                    supports_tracking_attributes,
                    supports_clockout,
                    is_active,
                    created_utc
                FROM core.workstation_type
                WHERE workstation_type_id = @id";

            // create a parameter for the ID and add it to the command.
            var parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.Value = id;
            command.Parameters.Add(parameter);

            // execute the command and read the result using a data reader.
            using var reader = await command.ExecuteReaderAsync();

            // if a workstation type is found, create a WorkStationType object and return it, otherwise return null.
            if (await reader.ReadAsync())
            {
                return new WorkStationType
                {
                    WorkStationTypeId = reader.GetInt32(reader.GetOrdinal("workstation_type_id")),
                    WorkStationTypeName = reader.GetString(reader.GetOrdinal("workstation_type_name")),
                    WorkStationTypeCode = reader.GetString(reader.GetOrdinal("workstation_type_code")),
                    SupportsCpk = reader.GetBoolean(reader.GetOrdinal("supports_cpk")),
                    SupportsRejectEntry = reader.GetBoolean(reader.GetOrdinal("supports_reject_entry")),
                    SupportsTrackingAttributes = reader.GetBoolean(reader.GetOrdinal("supports_tracking_attributes")),
                    SupportsClockOut = reader.GetBoolean(reader.GetOrdinal("supports_clockout")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                };
            }

            // if no workstation type is found,
            return null;
        }

        // implement the InsertAsync method to insert a new workstation type to the database.
        public async Task<int> InsertAsync(WorkStationType workStationType)
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

            // create a database command to execute the SQL query to insert a new workstation type and return the new ID.   
            using var command = dbConnection.CreateCommand();

            // set the command text to insert a new workstation type into the core.workstation_type table and return the new workstation_type_id.
            command.CommandText = @"
                INSERT INTO core.workstation_type (workstation_type_name, workstation_type_code, supports_cpk, supports_reject_entry, supports_tracking_attributes, supports_clockout, is_active, created_utc)
                VALUES (@name, @code, @supports_cpk, @supports_reject_entry, @supports_tracking_attributes, @supports_clockout, @is_active, @created_utc)
                RETURNING workstation_type_id";

            // create parameters for the workstation and add them to command
            var nameparameter = command.CreateParameter();
            nameparameter.ParameterName = "@name";
            nameparameter.Value = workStationType.WorkStationTypeName;
            command.Parameters.Add(nameparameter);

            var codeparameter = command.CreateParameter();
            codeparameter.ParameterName = "@code";
            codeparameter.Value = workStationType.WorkStationTypeCode;
            command.Parameters.Add(codeparameter);

            var cpkparameter = command.CreateParameter();
            cpkparameter.ParameterName = "@supports_cpk";
            cpkparameter.Value = workStationType.SupportsCpk;
            command.Parameters.Add(cpkparameter);

            var rejectparameter = command.CreateParameter();
            rejectparameter.ParameterName = "@supports_reject_entry";
            rejectparameter.Value = workStationType.SupportsRejectEntry;
            command.Parameters.Add(rejectparameter);

            var trackingparameter = command.CreateParameter();
            trackingparameter.ParameterName = "@supports_tracking_attributes";
            trackingparameter.Value = workStationType.SupportsTrackingAttributes;
            command.Parameters.Add(trackingparameter);

            var clockoutparameter = command.CreateParameter();
            clockoutparameter.ParameterName = "@supports_clockout";
            clockoutparameter.Value = workStationType.SupportsClockOut;
            command.Parameters.Add(clockoutparameter);

            var isactiveparameter = command.CreateParameter();
            isactiveparameter.ParameterName = "@is_active";
            isactiveparameter.Value = workStationType.IsActive;
            command.Parameters.Add(isactiveparameter);

            var createdutcparameter = command.CreateParameter();
            createdutcparameter.ParameterName = "@created_utc";
            createdutcparameter.Value = workStationType.CreatedUtc;
            command.Parameters.Add(createdutcparameter);

            // execute the command and retrieve the new workstation_type_id.
            object? result = await command.ExecuteScalarAsync();

            // check if the result is null or DBNull, if so throw an exception, otherwise convert the result to an integer and return it as the new workstation_type_id.
            if (result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException("Failed to insert workstationtype and retrieve the new ID.");
            }

            // convert the result to an integer and return it as the new workstation_type_id.
            return Convert.ToInt32(result);
        }

        // implement the UpdateAsync method to update a existing workstation type in the database by ID.
        public async Task<bool> UpdateAsync(WorkStationType workStationType)
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

            // create a database command to execute the SQL query to update an existing workstation type by ID.
            using var command = dbConnection.CreateCommand();

            // set the command text to update an existing workstation type in the core.workstation_type table where the workstation_type_id matches the provided ID.
            command.CommandText = @"
            UPDATE core.workstation_type
            SET workstation_type_name = @name,
                workstation_type_code = @code,
                supports_cpk = @cpk,
                supports_reject_entry = @reject,
                supports_tracking_attributes = @tracking,
                supports_clockout = @clockout,
                is_active = @active,
                created_utc = @created_utc
            WHERE workstation_type_id = @id";

            // create parameters for the workstation and add them to command
            var nameparameter = command.CreateParameter();
            nameparameter.ParameterName = "@name";
            nameparameter.Value = workStationType.WorkStationTypeName;
            command.Parameters.Add(nameparameter);

            var codeparameter = command.CreateParameter();
            codeparameter.ParameterName = "@code";
            codeparameter.Value = workStationType.WorkStationTypeCode;
            command.Parameters.Add(codeparameter);

            var createdutcparameter = command.CreateParameter();
            createdutcparameter.ParameterName = "@created_utc";
            createdutcparameter.Value = workStationType.CreatedUtc;
            command.Parameters.Add(createdutcparameter);

            var cpkparameter = command.CreateParameter();
            cpkparameter.ParameterName = "@cpk";
            cpkparameter.Value = workStationType.SupportsCpk;
            command.Parameters.Add(cpkparameter);

            var rejectparameter = command.CreateParameter();
            rejectparameter.ParameterName = "@reject";
            rejectparameter.Value = workStationType.SupportsRejectEntry;
            command.Parameters.Add(rejectparameter);

            var trackingparameter = command.CreateParameter();
            trackingparameter.ParameterName = "@tracking";
            trackingparameter.Value = workStationType.SupportsTrackingAttributes;
            command.Parameters.Add(trackingparameter);

            var clockoutparameter = command.CreateParameter();
            clockoutparameter.ParameterName = "@clockout";
            clockoutparameter.Value = workStationType.SupportsClockOut;
            command.Parameters.Add(clockoutparameter);

            var isactiveparameter = command.CreateParameter();
            isactiveparameter.ParameterName = "@active";
            isactiveparameter.Value = workStationType.IsActive;
            command.Parameters.Add(isactiveparameter);

            var idparameter = command.CreateParameter();
            idparameter.ParameterName = "@id";
            idparameter.Value = workStationType.WorkStationTypeId;
            command.Parameters.Add(idparameter);

            // execute the command and check the number of rows affected to determine if the update was successful.
            int rowsAffected = await command.ExecuteNonQueryAsync();

            // return true if at least one row was affected, indicating that the update was successful, otherwise return false.
            return rowsAffected > 0;
        }

        // implement the DeleteAsync method to delete existing workstation type from database by ID.
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

            // create a database command to execute the SQL query to delete an existing workstation type by ID.
            using var command = dbConnection.CreateCommand();

            // set the command text to delete an existing workstation type from the core.workstation_type table where the workstation_type_id matches the provided ID.
            command.CommandText = @"
            DELETE FROM core.workstation_type
            WHERE workstation_type_id = @id";

            // create a parameter for the ID and add it to the command.
            var idparameter = command.CreateParameter();
            idparameter.ParameterName = "@id";
            idparameter.Value = id;
            command.Parameters.Add(idparameter);

            // execute the command and check the number of rows affected to determine if the delete was successful.
            int rowsAffected = await command.ExecuteNonQueryAsync();

            // return true if at least one row was affected, indicating that the delete was successful, otherwise return false.
            return rowsAffected > 0;
        }
    }
}
