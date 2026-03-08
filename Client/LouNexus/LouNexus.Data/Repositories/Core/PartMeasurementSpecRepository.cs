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
    public class PartMeasurementSpecRepository : IPartMeasurementSpecRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;
    
            public PartMeasurementSpecRepository(IDbConnectionProvider connectionProvider)
            {
                _connectionProvider = connectionProvider;
            }
    
            public Task<IEnumerable<PartMeasurementSpec>> GetAllAsync()
            {
                throw new NotImplementedException();
            }
    
            public Task<PartMeasurementSpec?> GetByIdAsync(int id)
            {
                throw new NotImplementedException();
            }
    
            public Task<int> InsertAsync(PartMeasurementSpec partMeasurementSpec)
            {
                throw new NotImplementedException();
            }
    
            public Task<bool> UpdateAsync(PartMeasurementSpec partMeasurementSpec)
            {
                throw new NotImplementedException();
            }
    
            public Task<bool> DeleteAsync(int id)
            {
                throw new NotImplementedException();
        }
    }
}
