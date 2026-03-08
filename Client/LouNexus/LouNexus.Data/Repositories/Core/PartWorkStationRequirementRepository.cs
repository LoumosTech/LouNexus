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
    public class PartWorkStationRequirementRepository : IPartWorkStationRequirementRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;
        public PartWorkStationRequirementRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }
        public Task<IEnumerable<PartWorkStationRequirement>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public Task<PartWorkStationRequirement?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<int> InsertAsync(PartWorkStationRequirement partWorkStationRequirement)
        {
            throw new NotImplementedException();
        }
        public Task<bool> UpdateAsync(PartWorkStationRequirement partWorkStationRequirement)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
