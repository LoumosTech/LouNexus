using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LouNexus.Core.Models.Core;

namespace LouNexus.Core.Interfaces.Core
{
    public interface IWorkStationRepository
    {
            Task<IEnumerable<WorkStation>> GetAllAsync();
            Task<WorkStation?> GetByIdAsync(int id);
            Task<int> InsertAsync(WorkStation workStation);
            Task<bool> UpdateAsync(WorkStation workStation);
            Task<bool> DeleteAsync(int id);
    }
}
