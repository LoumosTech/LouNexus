using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LouNexus.Core.Models.Core;

namespace LouNexus.Core.Interfaces.Core
{
    public interface IPartRepository
    {
        Task<IEnumerable<Part>> GetAllAsync();
        Task<Part?> GetByIdAsync(int id);
        Task<int> InsertAsync(Part part);
        Task<bool> UpdateAsync(Part part);
        Task<bool> DeleteAsync(int id);
    }
}
