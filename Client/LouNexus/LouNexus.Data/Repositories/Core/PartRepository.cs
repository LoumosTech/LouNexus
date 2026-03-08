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
    public class PartRepository : IPartRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;

        public PartRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public Task<IEnumerable<Part>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Part?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync(Part part)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Part part)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }   
    }
}
