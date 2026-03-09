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
    public class PartRepository : IPartRepository
    {
        //define the connection provider
        private readonly IDbConnectionProvider _connectionProvider;


        public PartRepository(IDbConnectionProvider connectionProvider)
        {
            //assign the connection provider
            _connectionProvider = connectionProvider;
        }

        //implement the GetAllAsync method to retrieve all parts from the database
        public async Task<IEnumerable<Part>> GetAllAsync()
        {
            //create a list to hold the parts
            var parts = new List<Part>();

            //create a connection to the database
            using var connection = _connectionProvider.CreateConnection();

            //check if the connection is a DbConnection
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            //open the connection
            await dbConnection.OpenAsync();

            //create a command to execute the query
            using var command = dbConnection.CreateCommand();

            //set the command text to select all parts
            command.CommandText = @"
                SELECT 
                    part_id,
                    part_number,
                    part_name,
                    part_description,
                    print_url,
                    is_active,
                    created_utc
                FROM core.part";

            //execute the command and read the results
            using var reader = await command.ExecuteReaderAsync();

            //loop through the results and add them to the list
            while (await reader.ReadAsync())
            {
                //create a new part object, populate with data from db and add it to the list
                parts.Add(new Part
                {
                    PartId = reader.GetInt32(reader.GetOrdinal("part_id")),
                    PartNumber = reader.GetString(reader.GetOrdinal("part_number")),
                    PartName = reader.GetString(reader.GetOrdinal("part_name")),
                    PartDescription = reader.GetString(reader.GetOrdinal("part_description")),
                    PrintUrl = reader.IsDBNull(reader.GetOrdinal("print_url")) ? null : reader.GetString(reader.GetOrdinal("print_url")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                });
            }

            //return the list of parts
            return parts;
        }

        //implement the GetByIdAsync method to retrieve a part by id from the database
        public async Task<Part?> GetByIdAsync(int id)
        {
            //create a connection to the database
            using var connection = _connectionProvider.CreateConnection();

            //check if the connection is a DbConnection
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            //open the connection
            await dbConnection.OpenAsync();

            //create a command to execute the query
            using var command = dbConnection.CreateCommand();

            //set the command text to select a part by id
            command.CommandText = @"
                SELECT 
                    part_id,
                    part_number,
                    part_name,
                    part_description,
                    print_url,
                    is_active,
                    created_utc
                FROM core.part
                WHERE part_id = @id";

            //create a parameter for the id and add it to the command
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            //execute the command and read the result
            using var reader = await command.ExecuteReaderAsync();

            //if a part is found, create a new part object, populate with data from db and return it
            if (await reader.ReadAsync())
            {
                //create a new part object, populate with data from db and return it
                return new Part
                {
                    PartId = reader.GetInt32(reader.GetOrdinal("part_id")),
                    PartNumber = reader.GetString(reader.GetOrdinal("part_number")),
                    PartName = reader.GetString(reader.GetOrdinal("part_name")),
                    PartDescription = reader.GetString(reader.GetOrdinal("part_description")),
                    PrintUrl = reader.IsDBNull(reader.GetOrdinal("print_url")) ? null : reader.GetString(reader.GetOrdinal("print_url")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                };
            }

            //if no part is found, return null
            return null;
        }

        //implement the InsertAsync method to insert a new part into the database and return the new id
        public async Task<int> InsertAsync(Part part)
        {
            //create a connection to the database
            using var connection = _connectionProvider.CreateConnection();

            //check if the connection is a DbConnection
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            //open the connection
            await dbConnection.OpenAsync();

            //create a command to execute the query
            using var command = dbConnection.CreateCommand();

            //set the command text to insert a new part and return the new id
            command.CommandText = @"
                INSERT INTO core.part (
                    part_number,
                    part_name,
                    part_description,
                    print_url,
                    is_active,
                    created_utc
                ) VALUES (
                    @part_number,
                    @part_name,
                    @part_description,
                    @print_url,
                    @is_active,
                    @created_utc
                )
                RETURNING part_id";

            //create parameters for the part properties and add them to the command
            var partNumberParameter = command.CreateParameter();
            partNumberParameter.ParameterName = "@part_number";
            partNumberParameter.Value = part.PartNumber;
            command.Parameters.Add(partNumberParameter);

            var partNameParameter = command.CreateParameter();
            partNameParameter.ParameterName = "@part_name";
            partNameParameter.Value = part.PartName;
            command.Parameters.Add(partNameParameter);

            var partDescriptionParameter = command.CreateParameter();
            partDescriptionParameter.ParameterName = "Description";
            partDescriptionParameter.Value = part.PartDescription;
            command.Parameters.Add(partDescriptionParameter);

            var printUrlParameter = command.CreateParameter();
            printUrlParameter.ParameterName = "@print_url";
            printUrlParameter.Value = (object?)part.PrintUrl ?? DBNull.Value;
            command.Parameters.Add(printUrlParameter);

            var isActiveParameter = command.CreateParameter();
            isActiveParameter.ParameterName = "@is_active";
            isActiveParameter.Value = part.IsActive;
            command.Parameters.Add(isActiveParameter);

            var createdUtcParameter = command.CreateParameter();
            createdUtcParameter.ParameterName = "@created_utc";
            createdUtcParameter.Value = part.CreatedUtc;
            command.Parameters.Add(createdUtcParameter);

            //execute the command and get the new id
            var result = await command.ExecuteScalarAsync();

            //check if the result is null or DBNull and throw an exception if it is
            if (result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException("Failed to insert the part and retrieve the new ID.");
            }

            //return the new id as an integer
            return Convert.ToInt32(result);
        }

        //implement the UpdateAsync method to update an existing part in the database and return true if successful
        public async Task<bool> UpdateAsync(Part part)
        {
            //create a connection to the database
            using var connection = _connectionProvider.CreateConnection();

            //check if the connection is a DbConnection
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            //open the connection
            await dbConnection.OpenAsync();

            //create a command to execute the query
            using var command = dbConnection.CreateCommand();

            //set the command text to update an existing part
            command.CommandText = @"
                UPDATE core.part
                SET 
                    part_number = @part_number,
                    part_name = @part_name,
                    part_description = @part_description,
                    print_url = @print_url,
                    is_active = @is_active
                WHERE part_id = @part_id";

            //create parameters for the part properties and add them to the command
            var partNumberParameter = command.CreateParameter();
            partNumberParameter.ParameterName = "@part_number";
            partNumberParameter.Value = part.PartNumber;
            command.Parameters.Add(partNumberParameter);

            var partNameParameter = command.CreateParameter();
            partNameParameter.ParameterName = "@part_name";
            partNameParameter.Value = part.PartName;
            command.Parameters.Add(partNameParameter);

            var partDescriptionParameter = command.CreateParameter();
            partDescriptionParameter.ParameterName = "@part_description";
            partDescriptionParameter.Value = part.PartDescription;
            command.Parameters.Add(partDescriptionParameter);

            var printUrlParameter = command.CreateParameter();
            printUrlParameter.ParameterName = "@print_url";
            printUrlParameter.Value = (object?)part.PrintUrl ?? DBNull.Value;
            command.Parameters.Add(printUrlParameter);

            var isActiveParameter = command.CreateParameter();
            isActiveParameter.ParameterName = "@is_active";
            isActiveParameter.Value = part.IsActive;
            command.Parameters.Add(isActiveParameter);

            var partIdParameter = command.CreateParameter();
            partIdParameter.ParameterName = "@part_id";
            partIdParameter.Value = part.PartId;
            command.Parameters.Add(partIdParameter);

            //execute the command and get the number of rows affected
            int rowsAffected = await command.ExecuteNonQueryAsync();

            //return true if at least one row was affected, otherwise return false
            return rowsAffected > 0;
        }

        //implement the DeleteAsync method to delete a part by id from the database and return true if successful
        public async Task<bool> DeleteAsync(int id)
        {
            //create a connection to the database
            var connection = _connectionProvider.CreateConnection();

            //check if the connection is a DbConnection
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            //open the connection
            await dbConnection.OpenAsync();

            //create a command to execute the query
            using var command = dbConnection.CreateCommand();

            //set the command text to delete a part by id
            command.CommandText = @"
                DELETE FROM core.part
                WHERE part_id = @id";

            //create a parameter for the id and add it to the command
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            //execute the command and get the number of rows affected
            int rowsAffected = await command.ExecuteNonQueryAsync();

            //return true if at least one row was affected, otherwise return false
            return rowsAffected > 0;
        }   
    }
}
