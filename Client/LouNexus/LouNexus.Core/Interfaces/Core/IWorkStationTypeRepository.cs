using LouNexus.Core.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Core.Interfaces.Core
{
    public interface IWorkStationTypeRepository
    {
        Task<IEnumerable<WorkStationType>> GetAllAsync();
        Task<WorkStationType?> GetByIdAsync(int id);
        Task<int> InsertAsync(WorkStationType workStationType);
        Task<bool> UpdateAsync(WorkStationType workStationType);
        Task<bool> DeleteAsync(int id);
    }
}
