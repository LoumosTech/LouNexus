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
    public class FactoryRepository : IFactoryRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;

        public FactoryRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public Task<IEnumerable<Factory>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Factory?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync(Factory factory)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Factory factory)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
