using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LouNexus.Core.Interfaces.Quality;
using LouNexus.Core.Models.Quality;
using LouNexus.Data.DataBase;

namespace LouNexus.Data.Repositories.Quality
{
    public class MeasurementValueRepository : IMeasurementValueRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;
        public MeasurementValueRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }
        public Task<IEnumerable<MeasurementValue>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public Task<MeasurementValue?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<int> InsertAsync(MeasurementValue measurementValue)
        {
            throw new NotImplementedException();
        }
        public Task<bool> UpdateAsync(MeasurementValue measurementValue)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
