using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LouNexus.Core.Interfaces.Core;
using LouNexus.Core.Models.Core;
using LouNexus.Data.DataBase;

namespace LouNexus.Data.Repositories.Core
{
    public class PartMeasurementSpecRepository : IPartMeasurementSpecRepository
    {
        // define connection provider
        private readonly IDbConnectionProvider _connectionProvider;
        

        public PartMeasurementSpecRepository(IDbConnectionProvider connectionProvider)
        {
            // assign connection provider
            _connectionProvider = connectionProvider;
        }

        // implement the GetAllAsync method to retrieve all PartMeasurementSpec records from the database
        public Task<IEnumerable<PartMeasurementSpec>> GetAllAsync()
        {
            // create a list to hold the PartMeasurementSpecs.

            // create a connection to the database.

            // check if connection is a DbConnection, if not throw an exception.

            // open the database connection.

            // create a command to execute the SQL query.

            // set the command text to select all PartMeasurementSpec from the database.

            // execute the command and read the results.

            // loop through the results and create PartMeasurementSpec objects, adding them to the list.

            // return the list of PartMeasurementSpecs.

            throw new NotImplementedException();
        }

        // implement the GetByIdAsync method to retrieve a PartMeasurementSpec record by its ID from the database
        public Task<PartMeasurementSpec?> GetByIdAsync(int id)
        {
            // create a connection to the database.

            // check if connection is a DbConnection, if not throw an exception.

            // open the database connection.

            // create a command to execute the SQL query.

            // set the command text to select a PartMeasurementSpec by ID from the database.

            // create a parameter for the ID and add it to the command.

            // execute the command and read the result.

            // if a record is found, create a PartMeasurementSpec object and return it.

            // if no record is found, return null.

            throw new NotImplementedException();
        }

        // implement the InsertAsync method to insert a new PartMeasurementSpec record into the database
        public Task<int> InsertAsync(PartMeasurementSpec partMeasurementSpec)
        {
            // create a connection to the database.

            // check if connection is a DbConnection, if not throw an exception.

            // open the database connection.

            // create a command to execute the SQL query.

            // set the command text to insert a new PartMeasurementSpec into the database.

            // create parameters for each property of the PartMeasurementSpec and add them to the command.

            // execute the command and return the new record ID.

            throw new NotImplementedException();
        }

        // implement the UpdateAsync method to update an existing PartMeasurementSpec record in the database
        public Task<bool> UpdateAsync(PartMeasurementSpec partMeasurementSpec)
        {
            // create a connection to the database.

            // check if connection is a DbConnection, if not throw an exception.

            // open the database connection.

            // create a command to execute the SQL query.

            // set the command text to update an existing PartMeasurementSpec in the database.

            // create parameters for each property of the PartMeasurementSpec and add them to the command.

            // execute the command.

            // return true if the update was successful, otherwise return false.

            throw new NotImplementedException();
        }

        // implement the DeleteAsync method to delete a PartMeasurementSpec record by its ID from the database
        public Task<bool> DeleteAsync(int id)
        {
            // create a connection to the database.

            // check if connection is a DbConnection, if not throw an exception.

            // open the database connection.

            // create a command to execute the SQL query.

            // set the command text to delete a PartMeasurementSpec by ID from the database.

            // create a parameter for the ID and add it to the command.

            // execute the command.

            // return true if the delete was successful, otherwise return false.

            throw new NotImplementedException();
        }
    }
}
