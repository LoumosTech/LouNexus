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
    public class RejectCodeRepository : IRejectCodeRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;
        public RejectCodeRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }
        public Task<IEnumerable<RejectCode>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public Task<RejectCode?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<int> InsertAsync(RejectCode rejectCode)
        {
            throw new NotImplementedException();
        }
        public Task<bool> UpdateAsync(RejectCode rejectCode)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
