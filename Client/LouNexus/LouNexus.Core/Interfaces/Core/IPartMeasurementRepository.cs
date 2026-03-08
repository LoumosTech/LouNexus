using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Core.Interfaces.Core
{
    public interface IPartMeasurementRepository
    {
        Task<IEnumerable<Models.Core.PartMeasurement>> GetAllAsync();
        Task<Models.Core.PartMeasurement?> GetByIdAsync(int id);
        Task<int> InsertAsync(Models.Core.PartMeasurement partMeasurement);
        Task<bool> UpdateAsync(Models.Core.PartMeasurement partMeasurement);
        Task<bool> DeleteAsync(int id);
    }
}
