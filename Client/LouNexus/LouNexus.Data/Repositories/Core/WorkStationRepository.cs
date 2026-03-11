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
    public class WorkStationRepository : IWorkStationRepository
    {
        // define the connection provider
        private readonly IDbConnectionProvider _connectionProvider;

        public WorkStationRepository(IDbConnectionProvider connectionProvider)
        {
            // assign the connection provider.
            _connectionProvider = connectionProvider;
        }

        // implement the GetAllAsync method to retrieve all work stations from the database.
        public async Task<IEnumerable<WorkStation>> GetAllAsync()
        {
            // create a list to hold the work stations.
            var workStations = new List<WorkStation>();

            // create a database connection using the connection provider.
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

            // set the command text to select all work stations from the database.
            command.CommandText = @"
                SELECT
                    workstation_id, 
                    workstation_name, 
                    workstation_code, 
                    workstation_mode, 
                    factory_id, 
                    workstation_type_id, 
                    is_active, 
                    notes,
                    created_utc
                FROM core.work_station";

            // execute the command and get a data reader.
            using var reader = await command.ExecuteReaderAsync();

            // read each record from the data reader and create a WorkStation object, then add it to the list.
            while (await reader.ReadAsync())
            {
                var workStation = new WorkStation
                {
                    WorkStationId = reader.GetInt32(reader.GetOrdinal("workstation_id")),
                    WorkStationName = reader.GetString(reader.GetOrdinal("workstation_name")),
                    WorkStationCode = reader.GetString(reader.GetOrdinal("workstation_code")),
                    WorkStationMode = reader.GetString(reader.GetOrdinal("workstation_mode")),
                    FactoryId = reader.GetInt32(reader.GetOrdinal("factory_id")),
                    WorkStationTypeId = reader.GetInt32(reader.GetOrdinal("workstation_type_id")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                    Notes = reader.GetString(reader.GetOrdinal("notes")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                };
                workStations.Add(workStation);
            }

            // return the list of work stations.
            return workStations;
        }

        // implement the GetByIdAsync method to retrieve a work station by its ID from the database.
        public async Task<WorkStation?> GetByIdAsync(int id)
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

            // create a command to execute the SQL query.
            using var command = dbConnection.CreateCommand();

            // set the command text to select a work station by its ID from the database.
            command.CommandText = @"
                SELECT
                    workstation_id, 
                    workstation_name, 
                    workstation_code, 
                    workstation_mode, 
                    factory_id, 
                    workstation_type_id, 
                    is_active, 
                    notes,
                    created_utc
                FROM core.work_station
                WHERE workstation_id = @id";

            // create a parameter for the ID and add it to the command.
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            // execute the command and get a data reader.
            using var reader = await command.ExecuteReaderAsync();

            // read the record from the data reader and create a WorkStation object, then return it. If no record is found, return null.
            if (await reader.ReadAsync())
            {
                var workStation = new WorkStation
                {
                    WorkStationId = reader.GetInt32(reader.GetOrdinal("workstation_id")),
                    WorkStationName = reader.GetString(reader.GetOrdinal("workstation_name")),
                    WorkStationCode = reader.GetString(reader.GetOrdinal("workstation_code")),
                    WorkStationMode = reader.GetString(reader.GetOrdinal("workstation_mode")),
                    FactoryId = reader.GetInt32(reader.GetOrdinal("factory_id")),
                    WorkStationTypeId = reader.GetInt32(reader.GetOrdinal("workstation_type_id")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                    Notes = reader.GetString(reader.GetOrdinal("notes")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                };
                return workStation;
            }

            // if no record is found, return null.
            return null;
        }

        // implement the InsertAsync method to insert a new work station into the database and return the new ID.
        public async Task<int> InsertAsync(WorkStation workStation)
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

            // create a command to execute the SQL query.
            using var command = dbConnection.CreateCommand();

            // set the command text to insert a new work station into the database and return the new ID.
            command.CommandText = @"
                INSERT INTO core.work_station 
                    (workstation_name, workstation_code, workstation_mode, factory_id, workstation_type_id, is_active, notes, created_utc)
                VALUES 
                    (@name, @code, @mode, @factoryId, @typeId, @isActive, @notes, @createdUtc)
                RETURNING workstation_id";

            // create parameters for the work station properties and add them to the command.
            var nameParameter = command.CreateParameter();
            nameParameter.ParameterName = "@name";
            nameParameter.Value = workStation.WorkStationName;
            command.Parameters.Add(nameParameter);

            var codeParameter = command.CreateParameter();
            codeParameter.ParameterName = "@code";
            codeParameter.Value = workStation.WorkStationCode;
            command.Parameters.Add(codeParameter);

            var modeParameter = command.CreateParameter();
            modeParameter.ParameterName = "@mode";
            modeParameter.Value = workStation.WorkStationMode;
            command.Parameters.Add(modeParameter);

            var factoryIdParameter = command.CreateParameter();
            factoryIdParameter.ParameterName = "@factoryId";
            factoryIdParameter.Value = workStation.FactoryId;
            command.Parameters.Add(factoryIdParameter);

            var typeIdParameter = command.CreateParameter();
            typeIdParameter.ParameterName = "@typeId";
            typeIdParameter.Value = workStation.WorkStationTypeId;
            command.Parameters.Add(typeIdParameter);

            var isActiveParameter = command.CreateParameter();
            isActiveParameter.ParameterName = "@isActive";
            isActiveParameter.Value = workStation.IsActive;
            command.Parameters.Add(isActiveParameter);

            var notesParameter = command.CreateParameter();
            notesParameter.ParameterName = "@notes";
            notesParameter.Value = workStation.Notes;
            command.Parameters.Add(notesParameter);

            var createdUtcParameter = command.CreateParameter();
            createdUtcParameter.ParameterName = "@createdUtc";
            createdUtcParameter.Value = workStation.CreatedUtc;
            command.Parameters.Add(createdUtcParameter);

            // execute the command and get the new ID.
            object? result = await command.ExecuteScalarAsync();

            // check if the result is null or DBNull, if so throw an exception.
            if (result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException("Failed to insert the work station and retrieve the new ID.");
            }

            // return the new ID as an integer.
            return Convert.ToInt32(result);
        }

        // implement the UpdateAsync method to update an existing work station in the database and return a boolean indicating success.
        public async Task<bool> UpdateAsync(WorkStation workStation)
        {
            // create a connection to the database.
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection.
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query.
            using var command = dbConnection.CreateCommand();

            // set the command text to update workstation from database by ID.
            command.CommandText = @"
                UPDATE core.work_station
                SET 
                    workstation_name = @name,
                    workstation_code = @code,
                    workstation_mode = @mode,
                    factory_id = @factory_id,
                    workstation_type_id = @type_id,
                    is_active = @is_active,
                    notes = @notes
                WHERE workstation_id = @id";

            // create and set paramiters for fields to update in database.
            var nameParameter = command.CreateParameter();
            nameParameter.ParameterName = "@name";
            nameParameter.Value = workStation.WorkStationName;
            command.Parameters.Add(nameParameter);

            var createdUtcParameter = command.CreateParameter();
            createdUtcParameter.ParameterName = "@createdUtc";
            createdUtcParameter.Value = workStation.CreatedUtc;
            command.Parameters.Add(createdUtcParameter);

            var codeParameter = command.CreateParameter();
            codeParameter.ParameterName = "@code";
            codeParameter.Value = workStation.WorkStationCode;
            command.Parameters.Add(codeParameter);

            var modeParameter = command.CreateParameter();
            modeParameter.ParameterName = "@mode";
            modeParameter.Value = workStation.WorkStationMode;
            command.Parameters.Add(modeParameter);

            var factoryIdParameter = command.CreateParameter();
            factoryIdParameter.ParameterName = "@factory_id";
            factoryIdParameter.Value = workStation.FactoryId;
            command.Parameters.Add(factoryIdParameter);

            var workStationTypeIdParameter = command.CreateParameter();
            workStationTypeIdParameter.ParameterName = "@type_id";
            workStationTypeIdParameter.Value = workStation.WorkStationTypeId;
            command.Parameters.Add(workStationTypeIdParameter);

            var isActiveParameter = command.CreateParameter();
            isActiveParameter.ParameterName = "@is_active";
            isActiveParameter.Value = workStation.IsActive;
            command.Parameters.Add(isActiveParameter);

            var notesParameter = command.CreateParameter();
            notesParameter.ParameterName = "@notes";
            notesParameter.Value = workStation.Notes;
            command.Parameters.Add(notesParameter);

            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = workStation.WorkStationId;
            command.Parameters.Add(idParameter);

            // get number of rows effected if sucessfull.
            int rowsAffected = await command.ExecuteNonQueryAsync();

            // if at least one row was affected, the update was successful, so return true, otherwise return false
            return rowsAffected > 0;
        }

        // implement the DeleteAsync method to delete a work station by its ID from the database and return a boolean indicating success.
        public async Task<bool> DeleteAsync(int id)
        {
            // create a connection to the database
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection
            await dbConnection.OpenAsync();
            
            // create a command to execute the SQL query
            using var command = dbConnection.CreateCommand();

            // set the command text to delete a workstation from the database by its ID
            command.CommandText = @"
                DELETE FROM core.work_station
                WHERE workstation_id = @id";

            // create a parameter for the factory ID and add it to the command
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            // execute the command and check how many rows were affected
            int rowsAffected = await command.ExecuteNonQueryAsync();

            // if at least one row was affected, the delete was successful, so return true, otherwise return false
            return rowsAffected > 0;
        }
    }
}
