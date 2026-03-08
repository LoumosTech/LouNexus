using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LouNexus.Core.Interfaces.Prod;
using LouNexus.Core.Models.Prod;
using LouNexus.Data.DataBase;

namespace LouNexus.Data.Repositories.Prod
{
    public class StationEventAttributeRepository : IStationEventAttributeRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;
        public StationEventAttributeRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }
        public Task<IEnumerable<StationEventAttribute>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public Task<StationEventAttribute?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<int> InsertAsync(StationEventAttribute stationEventAttribute)
        {
            throw new NotImplementedException();
        }
        public Task<bool> UpdateAsync(StationEventAttribute stationEventAttribute)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
