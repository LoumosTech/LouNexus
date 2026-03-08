using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LouNexus.Core.Models.Core;

namespace LouNexus.Core.Interfaces.Core
{
    public interface IFactoryRepository
    {
        Task<IEnumerable<Factory>> GetAllAsync();
        Task<Factory?> GetByIdAsync(int id);
        Task<int> InsertAsync(Factory factory);
        Task<bool> UpdateAsync(Factory factory);
        Task<bool> DeleteAsync(int id);
    }
}
