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
    public class MeasurementSetRepository : IMeasurementSetRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;
        public MeasurementSetRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }
        public Task<IEnumerable<MeasurementSet>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public Task<MeasurementSet?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<int> InsertAsync(MeasurementSet measurementSet)
        {
            throw new NotImplementedException();
        }
        public Task<bool> UpdateAsync(MeasurementSet measurementSet)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
