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
    public class WorkStationTypeRepository : IWorkStationTypeRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;
        public WorkStationTypeRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }
        public Task<IEnumerable<WorkStationType>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public Task<WorkStationType?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<int> InsertAsync(WorkStationType workStationType)
        {
            throw new NotImplementedException();
        }
        public Task<bool> UpdateAsync(WorkStationType workStationType)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
