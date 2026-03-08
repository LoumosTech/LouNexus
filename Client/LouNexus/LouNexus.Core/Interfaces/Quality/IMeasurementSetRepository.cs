using LouNexus.Core.Models.Quality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Core.Interfaces.Quality
{
    public interface IMeasurementSetRepository
    {
        Task<IEnumerable<MeasurementSet>> GetAllAsync();
        Task<MeasurementSet?> GetByIdAsync(int id);
        Task<int> InsertAsync(MeasurementSet measurementSet);
        Task<bool> UpdateAsync(MeasurementSet measurementSet);
        Task<bool> DeleteAsync(int id);
    }
}
