using LouNexus.Core.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouNexus.Core.Interfaces.Core
{
    public interface IPartTrackingAttributeRepository
    {
            Task<IEnumerable<PartTrackingAttribute>> GetAllAsync();
            Task<PartTrackingAttribute?> GetByIdAsync(int id);
            Task<int> InsertAsync(PartTrackingAttribute partTrackingAttribute);
            Task<bool> UpdateAsync(PartTrackingAttribute partTrackingAttribute);
            Task<bool> DeleteAsync(int id);
    }
}
