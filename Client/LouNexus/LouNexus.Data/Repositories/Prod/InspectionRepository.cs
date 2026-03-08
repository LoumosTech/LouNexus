using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LouNexus.Core.Interfaces.Prod;
using LouNexus.Core.Models.Prod;
using LouNexus.Data.DataBase;

namespace LouNexus.Data.Repositories.Prod
{
    public class InspectionRepository : IInspectionRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;
        public InspectionRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }
        public Task<IEnumerable<Inspection>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
        public Task<Inspection?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<int> InsertAsync(Inspection inspection)
        {
            throw new NotImplementedException();
        }
        public Task<bool> UpdateAsync(Inspection inspection)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
