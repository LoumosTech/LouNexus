using LouNexus.Core.Models.Prod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Core.Interfaces.Prod
{
    public interface IStationEventRepository
    {
        Task<IEnumerable<StationEvent>> GetAllAsync();
        Task<StationEvent?> GetByIdAsync(int id);
        Task<int> InsertAsync(StationEvent stationEvent);
        Task<bool> UpdateAsync(StationEvent stationEvent);
        Task<bool> DeleteAsync(int id);
    }
}
