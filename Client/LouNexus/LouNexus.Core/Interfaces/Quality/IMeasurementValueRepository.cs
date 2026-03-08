using LouNexus.Core.Models.Quality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Core.Interfaces.Quality
{
    public interface IMeasurementValueRepository
    {
        Task<IEnumerable<MeasurementValue>> GetAllAsync();
        Task<MeasurementValue?> GetByIdAsync(int id);
        Task<int> InsertAsync(MeasurementValue measurementValue);
        Task<bool> UpdateAsync(MeasurementValue measurementValue);
        Task<bool> DeleteAsync(int id);
    }
}
