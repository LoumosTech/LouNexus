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
    public class WorkStationRepository : IWorkStationRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;
        public WorkStationRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }
        public Task<IEnumerable<WorkStation>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public Task<WorkStation?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<int> InsertAsync(WorkStation workStation)
        {
            throw new NotImplementedException();
        }
        public Task<bool> UpdateAsync(WorkStation workStation)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
