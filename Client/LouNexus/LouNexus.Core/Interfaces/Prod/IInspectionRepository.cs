using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LouNexus.Core.Models.Prod;

namespace LouNexus.Core.Interfaces.Prod
{
    public interface IInspectionRepository
    {
        Task<IEnumerable<Inspection>> GetAllAsync();
        Task<Inspection?> GetByIdAsync(int id);
        Task<int> InsertAsync(Inspection inspection);
        Task<bool> UpdateAsync(Inspection inspection);
        Task<bool> DeleteAsync(int id);
    }
}
