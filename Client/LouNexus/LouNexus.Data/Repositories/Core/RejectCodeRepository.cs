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
    public class RejectCodeRepository : IRejectCodeRepository
    {
        // define the connection provider
        private readonly IDbConnectionProvider _connectionProvider;

        public RejectCodeRepository(IDbConnectionProvider connectionProvider)
        {
            // assign the connection provider
            _connectionProvider = connectionProvider;
        }

        // implement the GetAllAsync method to retrieve all reject codes from the database
        public async Task<IEnumerable<RejectCode>> GetAllAsync()
        {
            // create a list to hold the reject codes
            var RegectCodes = new List<RejectCode>();

            // create a database connection using the connection provider
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection and throw an exception if not
            if (connection is not DbConnection DbConnction)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection asynchronously
            await DbConnction.OpenAsync();

            // create a database command to execute the SQL query
            using var command = DbConnction.CreateCommand();

            // set the command text to select all reject codes from the database
            command.CommandText = @"
                SELECT 
                    reject_code_id, 
                    code, 
                    description, 
                    is_active, 
                    created_utc
                FROM core.reject_code";

            // execute the command and get a data reader to read the results
            using var reader = await command.ExecuteReaderAsync();

            // read each row from the data reader and create a RejectCode object for each row, then add it to the list
            while (await reader.ReadAsync())
            {
                var rejectCode = new RejectCode
                {
                    RejectCodeId = reader.GetInt32(reader.GetOrdinal("reject_code_id")),
                    Code = reader.GetString(reader.GetOrdinal("code")),
                    Description = reader.GetString(reader.GetOrdinal("description")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                };
                RegectCodes.Add(rejectCode);
            }

            // return the list of reject codes
            return RegectCodes;
        }

        // implement the GetByIdAsync method to retrieve a reject code by its ID from the database
        public async Task<RejectCode?> GetByIdAsync(int id)
        {
            // create a database connection using the connection provider
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection and throw an exception if not
            if (connection is not DbConnection DbConnction)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection asynchronously
            await DbConnction.OpenAsync();

            // create a database command to execute the SQL query
            using var command = DbConnction.CreateCommand();

            // set the command text to select a reject code by its ID from the database
            command.CommandText = @"
                SELECT 
                    reject_code_id, 
                    code, 
                    description, 
                    is_active, 
                    created_utc
                FROM core.reject_code
                WHERE reject_code_id = @id";

            // create a parameter for the ID and add it to the command
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            // execute the command and get a data reader to read the result
            using var reader = await command.ExecuteReaderAsync();

            // read the row from the data reader and create a RejectCode object if a row is found, then return it
            if (await reader.ReadAsync())
            {
                var rejectCode = new RejectCode
                {
                    RejectCodeId = reader.GetInt32(reader.GetOrdinal("reject_code_id")),
                    Code = reader.GetString(reader.GetOrdinal("code")),
                    Description = reader.GetString(reader.GetOrdinal("description")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                };
                return rejectCode;
            }

            // return null if no row is found
            return null;
        }

        // implement the InsertAsync method to insert a new reject code into the database and return the new ID
        public async Task<int> InsertAsync(RejectCode rejectCode)
        {
            // create a database connection using the connection provider
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection and throw an exception if not
            if (connection is not DbConnection DbConnction)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection asynchronously
            await DbConnction.OpenAsync();

            // create a database command to execute the SQL query
            using var command = DbConnction.CreateCommand();

            // set the command text to insert a new reject code into the database and return the new ID
            command.CommandText = @"
                INSERT INTO core.reject_code (code, description, is_active, created_utc)
                VALUES (@code, @description, @is_active, @created_utc)
                RETURNING reject_code_id";

            // create parameters for the reject code properties and add them to the command
            var codeParameter = command.CreateParameter();
            codeParameter.ParameterName = "@code";
            codeParameter.Value = rejectCode.Code;
            command.Parameters.Add(codeParameter);

            var descriptionParameter = command.CreateParameter();
            descriptionParameter.ParameterName = "@description";
            descriptionParameter.Value = rejectCode.Description;
            command.Parameters.Add(descriptionParameter);

            var activeParameter = command.CreateParameter();
            activeParameter.ParameterName = "@is_active";
            activeParameter.Value = rejectCode.IsActive;
            command.Parameters.Add(activeParameter);

            var createdUtcParameter = command.CreateParameter();
            createdUtcParameter.ParameterName = "@created_utc";
            createdUtcParameter.Value = rejectCode.CreatedUtc;
            command.Parameters.Add(createdUtcParameter);

            // execute the command and get the new ID from the result
            object? result = await command.ExecuteScalarAsync();

            // check if the result is null or DBNull and throw an exception if it is
            if (result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException("Failed to insert the reject code and retrieve the new ID.");
            }

            // return the new ID as an integer
            return Convert.ToInt32(result);
        }

        // implement the UpdateAsync method to update an existing reject code in the database and return a boolean indicating success
        public async Task<bool> UpdateAsync(RejectCode rejectCode)
        {
            // create a database connection using the connection provider
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection and throw an exception if not
            if (connection is not DbConnection DbConnction)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection asynchronously
            await DbConnction.OpenAsync();

            // create a database command to execute the SQL query
            using var command = DbConnction.CreateCommand();

            // set the command text to update an existing reject code in the database by its ID
            command.CommandText = @"
                UPDATE core.reject_code
                SET code = @code,
                    description = @description,
                    is_active = @is_active
                WHERE reject_code_id = @id";

            // create parameters for the reject code properties and add them to the command
            var codeParameter = command.CreateParameter();
            codeParameter.ParameterName = "@code";
            codeParameter.Value = rejectCode.Code;
            command.Parameters.Add(codeParameter);

            var descriptionParameter = command.CreateParameter();
            descriptionParameter.ParameterName = "@description";
            descriptionParameter.Value = rejectCode.Description;
            command.Parameters.Add(descriptionParameter);

            var activeParameter = command.CreateParameter();
            activeParameter.ParameterName = "@is_active";
            activeParameter.Value = rejectCode.IsActive;
            command.Parameters.Add(activeParameter);

            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = rejectCode.RejectCodeId;
            command.Parameters.Add(idParameter);

            // execute the command and get the number of rows affected
            int rowsAffected = await command.ExecuteNonQueryAsync();

            // return true if at least one row was affected, indicating that the update was successful
            return rowsAffected > 0;
        }

        // implement the DeleteAsync method to delete a reject code by its ID from the database and return a boolean indicating success
        public async Task<bool> DeleteAsync(int id)
        {
            // create a database connection using the connection provider
            using var connection = _connectionProvider.CreateConnection();

            // check if the connection is a DbConnection and throw an exception if not
            if (connection is not DbConnection DbConnction)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection asynchronously
            await DbConnction.OpenAsync();

            // create a database command to execute the SQL query
            using var command = DbConnction.CreateCommand();

            // set the command text to delete a reject code by its ID from the database
            command.CommandText = @"
                DELETE FROM core.reject_code
                WHERE reject_code_id = @id";

            // create a parameter for the ID and add it to the command
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            // execute the command and get the number of rows affected
            int rowsAffected = await command.ExecuteNonQueryAsync();

            // return true if at least one row was affected, indicating that the delete was successful
            return rowsAffected > 0;
        }
    }
}
