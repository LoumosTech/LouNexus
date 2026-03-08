using LouNexus.Core.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Core.Interfaces.Core
{
    public interface IRejectCodeRepository
    {
        Task<IEnumerable<RejectCode>> GetAllAsync();
        Task<RejectCode?> GetByIdAsync(int id);
        Task<int> InsertAsync(RejectCode rejectCode);
        Task<bool> UpdateAsync(RejectCode rejectCode);
        Task<bool> DeleteAsync(int id);
    }
}
