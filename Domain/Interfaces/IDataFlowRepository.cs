using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IDataFlowRepository
    {
        Task<string> CreateAsync(string dataFlowJson);
        Task<string> GetByIdAsync(string id);
        Task UpdateAsync(string id, string dataFlowJson);
        Task DeleteAsync(string id);
    }
}
