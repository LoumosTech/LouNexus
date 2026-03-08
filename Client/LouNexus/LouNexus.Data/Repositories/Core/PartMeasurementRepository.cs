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
    public class PartMeasurementRepository : IPartMeasurementRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;

        public PartMeasurementRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public async Task<IEnumerable<PartMeasurement>> GetAllAsync()
        {
            // Implementation to retrieve all PartMeasurements from the database
            throw new NotImplementedException();
        }

        public async Task<PartMeasurement?> GetByIdAsync(int id)
        {
            // Implementation to retrieve a PartMeasurement by its ID from the database
            throw new NotImplementedException();
        }

        public async Task<int> InsertAsync(PartMeasurement partMeasurement)
        {
            // Implementation to insert a new PartMeasurement into the database and return its new ID
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(PartMeasurement partMeasurement)
        {
            // Implementation to update an existing PartMeasurement in the database
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            // Implementation to delete a PartMeasurement by its ID from the database
            throw new NotImplementedException();
        }
    }
}
