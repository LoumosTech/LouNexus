using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LouNexus.Core.Models.Inventory;

namespace LouNexus.Core.Interfaces.Inventory
{
    public interface IRawMaterialRepository
    {
        Task<IEnumerable<RawMaterial>> GetAllAsync();
        Task<RawMaterial?> GetByIdAsync(int id);
        Task<int> InsertAsync(RawMaterial rawMaterial);
        Task<bool> UpdateAsync(RawMaterial rawMaterial);
        Task<bool> DeleteAsync(int id);
    }
}
