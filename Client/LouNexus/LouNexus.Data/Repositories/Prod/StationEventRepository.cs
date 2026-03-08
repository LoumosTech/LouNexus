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
    public class StationEventRepository : IStationEventRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;
        public StationEventRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }
        public Task<IEnumerable<StationEvent>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public Task<StationEvent?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<int> InsertAsync(StationEvent stationEvent)
        {
            throw new NotImplementedException();
        }
        public Task<bool> UpdateAsync(StationEvent stationEvent)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
