using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LouNexus.Core.Models.Core;

namespace LouNexus.Core.Interfaces.Core
{
    public interface IPartMeasurementSpecRepository
    {
        Task<IEnumerable<PartMeasurementSpec>> GetAllAsync();
        Task<PartMeasurementSpec?> GetByIdAsync(int id);
        Task<int> InsertAsync(PartMeasurementSpec partMeasurementSpec);
        Task<bool> UpdateAsync(PartMeasurementSpec partMeasurementSpec);
        Task<bool> DeleteAsync(int id);
    }
}
