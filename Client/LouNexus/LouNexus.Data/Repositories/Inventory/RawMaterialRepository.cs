using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LouNexus.Core.Interfaces.Inventory;
using LouNexus.Core.Models.Inventory;
using LouNexus.Data.DataBase;

namespace LouNexus.Data.Repositories.Inventory
{
    public class RawMaterialRepository : IRawMaterialRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;
        public RawMaterialRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }
        public Task<IEnumerable<RawMaterial>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public Task<RawMaterial?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<int> InsertAsync(RawMaterial rawMaterial)
        {
            throw new NotImplementedException();
        }
        public Task<bool> UpdateAsync(RawMaterial rawMaterial)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
