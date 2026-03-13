using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LouNexus.Core.Interfaces.Inventory;
using LouNexus.Core.Models.Inventory;
using LouNexus.Data.DataBase;

namespace LouNexus.Data.Repositories.Inventory
{
    public class RawMaterialRepository : IRawMaterialRepository
    {
        // define a connection provider to manage database connections
        private readonly IDbConnectionProvider _connectionProvider;
        public RawMaterialRepository(IDbConnectionProvider connectionProvider)
        {
            // assign the connection provider through dependency injection
            _connectionProvider = connectionProvider;
        }

        // implement the methods defined in the IRawMaterialRepository interface
        public async Task<IEnumerable<RawMaterial>> GetAllAsync()
        {
            // initialize a list to hold the retrieved raw materials
            var RawMaterials = new List<RawMaterial>();

            // create a database connection using the connection provider
            using var connection = _connectionProvider.CreateConnection();

            // ensure the connection is of the expected type (e.g., SqlConnection) before opening it
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection asynchronously
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query for retrieving all raw materials
            using var command = dbConnection.CreateCommand();

            // set the command text to select all relevant fields from the raw_materials table
            command.CommandText = @"
                SELECT 
                    raw_material_id, 
                    part_id, 
                    quantity, 
                    lot_number, 
                    material_description, 
                    factory_id, 
                    is_active, 
                    created_utc
                FROM inventory.raw_material";

            // execute the command and obtain a data reader to read the results
            using var reader = await command.ExecuteReaderAsync();

            // read each record from the data reader and map it to a RawMaterial object, then add it to the list
            while (await reader.ReadAsync())
            {
                RawMaterials.Add(new RawMaterial
                {
                    RawMaterialId = reader.GetInt32(reader.GetOrdinal("raw_material_id")),
                    PartId = reader.GetInt32(reader.GetOrdinal("part_id")),
                    Quantity = reader.GetDecimal(reader.GetOrdinal("quantity")),
                    LotNumber = reader.GetString(reader.GetOrdinal("lot_number")),
                    MaterialDescription = reader.GetString(reader.GetOrdinal("material_description")),
                    FactoryId = reader.GetInt32(reader.GetOrdinal("factory_id")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                });
            }

            // return the list of raw materials retrieved from the database
            return RawMaterials;
        }
        // implement the method to retrieve a raw material by its ID
        public async Task<RawMaterial?> GetByIdAsync(int id)
        {
            // create a database connection using the connection provider
            using var connection = _connectionProvider.CreateConnection();

            // ensure the connection is of the expected type (e.g., SqlConnection) before opening it
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection asynchronously
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query for retrieving a raw material by its ID
            using var command = dbConnection.CreateCommand();

            // set the command text to select the relevant fields from the raw_materials table where the raw_material_id matches the provided ID
            command.CommandText = @"
                SELECT 
                    raw_material_id, 
                    part_id, 
                    quantity, 
                    lot_number, 
                    material_description, 
                    factory_id, 
                    is_active, 
                    created_utc
                FROM inventory.raw_material
                WHERE raw_material_id = @id";

            // create a parameter for the ID and add it to the command's parameters collection
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            // execute the command and obtain a data reader to read the result
            using var reader = await command.ExecuteReaderAsync();

            // read the record from the data reader and map it to a RawMaterial object if a record is found, otherwise return null
            if (await reader.ReadAsync())
            {
                return new RawMaterial
                {
                    RawMaterialId = reader.GetInt32(reader.GetOrdinal("raw_material_id")),
                    PartId = reader.GetInt32(reader.GetOrdinal("part_id")),
                    Quantity = reader.GetDecimal(reader.GetOrdinal("quantity")),
                    LotNumber = reader.GetString(reader.GetOrdinal("lot_number")),
                    MaterialDescription = reader.GetString(reader.GetOrdinal("material_description")),
                    FactoryId = reader.GetInt32(reader.GetOrdinal("factory_id")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("is_active")),
                    CreatedUtc = reader.GetDateTime(reader.GetOrdinal("created_utc"))
                };
            }

            // return null if no record is found with the provided ID
            return null;
        }

        // implement the method to insert a new raw material and return its generated ID
        public async Task<int> InsertAsync(RawMaterial rawMaterial)
        {
            // create a database connection using the connection provider
            using var connection = _connectionProvider.CreateConnection();

            // ensure the connection is of the expected type (e.g., SqlConnection) before opening it
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection asynchronously
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query for inserting a new raw material and retrieving the generated ID
            using var command = dbConnection.CreateCommand();

            // set the command text to insert a new record into the raw_materials table and return the generated raw_material_id
            command.CommandText = @"
                INSERT INTO inventory.raw_material
                    (part_id, quantity, lot_number, material_description, factory_id, is_active, created_utc)
                VALUES 
                    (@partId, @quantity, @lotNumber, @materialDescription, @factoryId, @isActive, @createdUtc);
                RETURNING raw_material_id;";

            // create parameters for each field of the raw material and add them to the command's parameters collection
            var partIdParameter = command.CreateParameter();
            partIdParameter.ParameterName = "@partId";
            partIdParameter.Value = rawMaterial.PartId;
            command.Parameters.Add(partIdParameter);

            var quantityParameter = command.CreateParameter();
            quantityParameter.ParameterName = "@quantity";
            quantityParameter.Value = rawMaterial.Quantity;
            command.Parameters.Add(quantityParameter);

            var lotNumberParameter = command.CreateParameter();
            lotNumberParameter.ParameterName = "@lotNumber";
            lotNumberParameter.Value = rawMaterial.LotNumber;
            command.Parameters.Add(lotNumberParameter);

            var materialDescriptionParameter = command.CreateParameter();
            materialDescriptionParameter.ParameterName = "@materialDescription";
            materialDescriptionParameter.Value = rawMaterial.MaterialDescription;
            command.Parameters.Add(materialDescriptionParameter);

            var factoryIdParameter = command.CreateParameter();
            factoryIdParameter.ParameterName = "@factoryId";
            factoryIdParameter.Value = rawMaterial.FactoryId.HasValue ? (object)rawMaterial.FactoryId.Value : DBNull.Value;
            command.Parameters.Add(factoryIdParameter);

            var isActiveParameter = command.CreateParameter();
            isActiveParameter.ParameterName = "@isActive";
            isActiveParameter.Value = rawMaterial.IsActive;
            command.Parameters.Add(isActiveParameter);

            var createdUtcParameter = command.CreateParameter();
            createdUtcParameter.ParameterName = "@createdUtc";
            createdUtcParameter.Value = rawMaterial.CreatedUtc;
            command.Parameters.Add(createdUtcParameter);

            // execute the command and retrieve the generated raw_material_id
            object? result = await command.ExecuteScalarAsync();

            // check if the result is null or DBNull, which indicates a failure to insert the record and retrieve the ID
            if (result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException("Failed to insert the raw material and retrieve the generated ID.");
            }

            // convert the result to an integer and return it as the generated raw_material_id
            return Convert.ToInt32(result);
        }

        // implement the method to update an existing raw material and return a boolean indicating success
        public async Task<bool> UpdateAsync(RawMaterial rawMaterial)
        {
            // create a database connection using the connection provider
            using var connection = _connectionProvider.CreateConnection();

            // ensure the connection is of the expected type (e.g., SqlConnection) before opening it
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection asynchronously
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query for updating an existing raw material based on its ID
            using var command = dbConnection.CreateCommand();

            // set the command text to update the relevant fields of the raw_materials table where the raw_material_id matches the provided ID
            command.CommandText = @"
                UPDATE inventory.raw_material
                SET 
                    part_id = @partId,
                    quantity = @quantity,
                    lot_number = @lotNumber,
                    material_description = @materialDescription,
                    factory_id = @factoryId,
                    is_active = @isActive,
                    created_utc = @createdUtc
                WHERE raw_material_id = @id";

            // create parameters for each field of the raw material, including the ID, and add them to the command's parameters collection
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = rawMaterial.RawMaterialId;
            command.Parameters.Add(idParameter);

            var partIdParameter = command.CreateParameter();
            partIdParameter.ParameterName = "@partId";
            partIdParameter.Value = rawMaterial.PartId;
            command.Parameters.Add(partIdParameter);

            var quantityParameter = command.CreateParameter();
            quantityParameter.ParameterName = "@quantity";
            quantityParameter.Value = rawMaterial.Quantity;
            command.Parameters.Add(quantityParameter);

            var lotNumberParameter = command.CreateParameter();
            lotNumberParameter.ParameterName = "@lotNumber";
            lotNumberParameter.Value = rawMaterial.LotNumber;
            command.Parameters.Add(lotNumberParameter);

            var materialDescriptionParameter = command.CreateParameter();
            materialDescriptionParameter.ParameterName = "@materialDescription";
            materialDescriptionParameter.Value = rawMaterial.MaterialDescription;
            command.Parameters.Add(materialDescriptionParameter);

            var factoryIdParameter = command.CreateParameter();
            factoryIdParameter.ParameterName = "@factoryId";
            factoryIdParameter.Value = rawMaterial.FactoryId.HasValue ? (object)rawMaterial.FactoryId.Value : DBNull.Value;
            command.Parameters.Add(factoryIdParameter);

            var isActiveParameter = command.CreateParameter();
            isActiveParameter.ParameterName = "@isActive";
            isActiveParameter.Value = rawMaterial.IsActive;
            command.Parameters.Add(isActiveParameter);

            var createdUtcParameter = command.CreateParameter();
            createdUtcParameter.ParameterName = "@createdUtc";
            createdUtcParameter.Value = rawMaterial.CreatedUtc;
            command.Parameters.Add(createdUtcParameter);

            // execute the command and retrieve the number of rows affected by the update operation
            int rowsAffected = await command.ExecuteNonQueryAsync();

            // return true if at least one row was affected, indicating a successful update, otherwise return false
            return rowsAffected > 0;
        }

        // implement the method to delete a raw material by its ID and return a boolean indicating success
        public async Task<bool> DeleteAsync(int id)
        {
            // create a database connection using the connection provider
            using var connection = _connectionProvider.CreateConnection();

            // ensure the connection is of the expected type (e.g., SqlConnection) before opening it
            if (connection is not DbConnection dbConnection)
            {
                throw new InvalidOperationException("The database connection must inherit from DbConnection.");
            }

            // open the database connection asynchronously
            await dbConnection.OpenAsync();

            // create a command to execute the SQL query for deleting a raw material based on its ID
            using var command = dbConnection.CreateCommand();

            // set the command text to delete the record from the raw_materials table where the raw_material_id matches the provided ID
            command.CommandText = @"
                DELETE FROM inventory.raw_material
                WHERE raw_material_id = @id";

            // create a parameter for the ID and add it to the command's parameters collection
            var idParameter = command.CreateParameter();
            idParameter.ParameterName = "@id";
            idParameter.Value = id;
            command.Parameters.Add(idParameter);

            // execute the command and retrieve the number of rows affected by the delete operation
            int rowsAffected = await command.ExecuteNonQueryAsync();

            // return true if at least one row was affected, indicating a successful deletion, otherwise return false
            return rowsAffected > 0;
        }
    }
}
