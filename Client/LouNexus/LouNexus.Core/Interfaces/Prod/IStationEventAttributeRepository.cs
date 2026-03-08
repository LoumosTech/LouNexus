using LouNexus.Core.Models.Prod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Core.Interfaces.Prod
{
    public interface IStationEventAttributeRepository
    {
        Task<IEnumerable<StationEventAttribute>> GetAllAsync();
        Task<StationEventAttribute?> GetByIdAsync(int id);
        Task<int> InsertAsync(StationEventAttribute stationEventAttribute);
        Task<bool> UpdateAsync(StationEventAttribute stationEventAttribute);
        Task<bool> DeleteAsync(int id);
    }
}
