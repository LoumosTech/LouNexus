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
    public class StationEventRejectRepository : IStationEventRejectRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;
        public StationEventRejectRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }
        public Task<IEnumerable<StationEventReject>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public Task<StationEventReject?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<int> InsertAsync(StationEventReject stationEventReject)
        {
            throw new NotImplementedException();
        }
        public Task<bool> UpdateAsync(StationEventReject stationEventReject)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
