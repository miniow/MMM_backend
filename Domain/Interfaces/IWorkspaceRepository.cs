using Domain.Entities;

namespace Domain.Interfaces
{


    public interface IWorkspaceRepository
    {
        Task<Workspace> GetByIdAsync(Guid id);
        Task<IEnumerable<Workspace>> GetByUserIdAsync(string userId);
        Task<Workspace> AddAsync(Workspace workspace);
        Task<Workspace> UpdateAsync(Workspace workspace);
        Task DeleteAsync(Guid id);
        Task<bool> SaveChangesAsync();
    }
}
