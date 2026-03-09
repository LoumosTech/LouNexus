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
    public class PartMeasurementRepository : IPartMeasurementRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;

        public PartMeasurementRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public async Task<IEnumerable<PartMeasurement>> GetAllAsync()
        {
            var partMeasurements = new List<PartMeasurement>();
            using var connection = _connectionProvider.CreateConnection();

            if(connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            await dbConnection.OpenAsync();

            using var command = dbConnection.CreateCommand();

            command.CommandText = @"
                SELECT 
                    part_measurement_id,
                    part_measurement_name,
                    is_active,
                    created_utc
                FROM core.part_measurement";

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                partMeasurements.Add(new PartMeasurement
                {
                    PartMeasurementId = reader.GetInt32(reader.GetOrdinal("part_measurement_id")),
                    PartMeasurementName = reader.GetString(reader.GetOrdinal("part_measurement_name")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                });
            }

            return partMeasurements;
        }

        public async Task<PartMeasurement?> GetByIdAsync(int id)
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
                    part_measurement_id,
                    part_measurement_name,
                    is_active,
                    created_utc
                FROM core.part_measurement
                WHERE part_measurement_id = @id";

            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                return new PartMeasurement
                {
                    PartMeasurementId = reader.GetInt32(reader.GetOrdinal("part_measurement_id")),
                    PartMeasurementName = reader.GetString(reader.GetOrdinal("part_measurement_name")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                };
            }

            return null;
        }

        public async Task<int> InsertAsync(PartMeasurement partMeasurement)
        {
            using var connection = _connectionProvider.CreateConnection();

            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            await dbConnection.OpenAsync();

            using var command = dbConnection.CreateCommand();

            command.CommandText = @"
                INSERT INTO core.part_measurement (part_measurement_name, is_active, created_utc)
                VALUES (@name, @isActive, @createdUtc)
                RETURNING part_measurement_id";

            var nameParameter = command.CreateParameter();
            nameParameter.ParameterName = "@name";
            nameParameter.Value = partMeasurement.PartMeasurementName;
            command.Parameters.Add(nameParameter);

            var isActiveParameter = command.CreateParameter();
            isActiveParameter.ParameterName = "@isActive";
            isActiveParameter.Value = partMeasurement.IsActive;
            command.Parameters.Add(isActiveParameter);

            var createdUtcParameter = command.CreateParameter();
            createdUtcParameter.ParameterName = "@createdUtc";
            createdUtcParameter.Value = partMeasurement.CreatedUtc;
            command.Parameters.Add(createdUtcParameter);

            var result = await command.ExecuteScalarAsync();

            if (result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException("Failed to insert PartMeasurement and retrieve the new ID.");
            }

            return Convert.ToInt32(result);
        }

        public async Task<bool> UpdateAsync(PartMeasurement partMeasurement)
        {
            using var connection = _connectionProvider.CreateConnection();

            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            await dbConnection.OpenAsync();

            using var command = dbConnection.CreateCommand();

            command.CommandText = @"
                UPDATE core.part_measurement
                SET part_measurement_name = @name,
                    is_active = @isActive,
                    created_utc = @createdUtc
                WHERE part_measurement_id = @id";

            var nameParameter = command.CreateParameter();
            nameParameter.ParameterName = "@name";
            nameParameter.Value = partMeasurement.PartMeasurementName;
            command.Parameters.Add(nameParameter);

            var isActiveParameter = command.CreateParameter();
            isActiveParameter.ParameterName = "@isActive";
            isActiveParameter.Value = partMeasurement.IsActive;
            command.Parameters.Add(isActiveParameter);

            var createdUtcParameter = command.CreateParameter();
            createdUtcParameter.ParameterName = "@createdUtc";
            createdUtcParameter.Value = partMeasurement.CreatedUtc;
            command.Parameters.Add(createdUtcParameter);

            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = partMeasurement.PartMeasurementId;
            command.Parameters.Add(idParameter);

            int rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _connectionProvider.CreateConnection();

            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            await dbConnection.OpenAsync();

            var command = dbConnection.CreateCommand();

            command.CommandText = @"
                DELETE FROM core.part_measurement
                WHERE part_measurement_id = @id";

            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            int rowsAffected = await command.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }
    }
}
