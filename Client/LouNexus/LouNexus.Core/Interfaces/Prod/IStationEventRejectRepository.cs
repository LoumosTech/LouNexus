using LouNexus.Core.Models.Prod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Core.Interfaces.Prod
{
    public interface IStationEventRejectRepository
    {
        Task<IEnumerable<StationEventReject>> GetAllAsync();
        Task<StationEventReject?> GetByIdAsync(int id);
        Task<int> InsertAsync(StationEventReject stationEventReject);
        Task<bool> UpdateAsync(StationEventReject stationEventReject);
        Task<bool> DeleteAsync(int id);
    }
}
