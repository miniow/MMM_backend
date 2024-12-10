using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IDataPipelineRepository
    {
        Task<DataPipeline> GetByIdAsync(Guid id);
        Task<IEnumerable<DataPipeline>> GetByUserIdAsync(string userId);
        Task<DataPipeline> AddAsync(DataPipeline dataPipeline);
        Task<DataPipeline> UpdateAsync(DataPipeline dataPipeline);
        Task DeleteAsync(Guid id);
        Task<bool> SaveChangesAsync();
    }
}
