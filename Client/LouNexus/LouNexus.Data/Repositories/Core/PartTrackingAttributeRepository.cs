using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LouNexus.Core.Interfaces.Core;
using LouNexus.Core.Models.Core;
using LouNexus.Data.DataBase;

namespace LouNexus.Data.Repositories.Core
{
    public class PartTrackingAttributeRepository : IPartTrackingAttributeRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;

        public PartTrackingAttributeRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public Task<IEnumerable<PartTrackingAttribute>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PartTrackingAttribute?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync(PartTrackingAttribute partTrackingAttribute)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(PartTrackingAttribute partTrackingAttribute)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
