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
        private readonly IDbConnectionProvider _connectionProvider;

        public PartRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public async Task<IEnumerable<Part>> GetAllAsync()
        {
            var parts = new List<Part>();

            using var connection = _connectionProvider.CreateConnection();

            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            await dbConnection.OpenAsync();

            using var command = dbConnection.CreateCommand();

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

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
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

            return parts;
        }

        public async Task<Part?> GetByIdAsync(int id)
        {
            using var connection = _connectionProvider.CreateConnection();

            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            await dbConnection.OpenAsync();

            using var command = dbConnection.CreateCommand();

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

            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
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

            return null;
        }

        public async Task<int> InsertAsync(Part part)
        {
            using var connection = _connectionProvider.CreateConnection();

            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            await dbConnection.OpenAsync();

            using var command = dbConnection.CreateCommand();

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

            var result = await command.ExecuteScalarAsync();

            if (result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException("Failed to insert the part and retrieve the new ID.");
            }

            return Convert.ToInt32(result);
        }

        public async Task<bool> UpdateAsync(Part part)
        {
            using var connection = _connectionProvider.CreateConnection();

            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            await dbConnection.OpenAsync();

            using var command = dbConnection.CreateCommand();

            command.CommandText = @"
                UPDATE core.part
                SET 
                    part_number = @part_number,
                    part_name = @part_name,
                    part_description = @part_description,
                    print_url = @print_url,
                    is_active = @is_active
                WHERE part_id = @part_id";

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

            int rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var connection = _connectionProvider.CreateConnection();

            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            await dbConnection.OpenAsync();

            using var command = dbConnection.CreateCommand();

            command.CommandText = @"
                DELETE FROM core.part
                WHERE part_id = @id";

            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            int rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }   
    }
}
