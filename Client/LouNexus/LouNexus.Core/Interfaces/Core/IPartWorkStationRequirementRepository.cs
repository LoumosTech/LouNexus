using LouNexus.Core.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Core.Interfaces.Core
{
    public interface IPartWorkStationRequirementRepository
    {
        Task<IEnumerable<PartWorkStationRequirement>> GetAllAsync();
        Task<PartWorkStationRequirement?> GetByIdAsync(int id);
        Task<int> InsertAsync(PartWorkStationRequirement partWorkStationRequirement);
        Task<bool> UpdateAsync(PartWorkStationRequirement partWorkStationRequirement);
        Task<bool> DeleteAsync(int id);
    }
}
