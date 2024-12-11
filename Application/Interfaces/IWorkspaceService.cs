using Application.DTOs;

namespace Application.Interfaces
{
    public interface IWorkspaceService
    {
        Task<WorkspaceDto> GetWorkspaceByIdAsync(Guid id);
        Task<IEnumerable<WorkspaceDto>> GetAllWorkspacesByUserIdAsync(string userId);
        Task<WorkspaceDto> CreateWorkspaceAsync(WorkspaceDto workspaceDto,string userId);
        Task<WorkspaceDto> UpdateWorkspaceAsync(WorkspaceDto workspaceDto);
        Task DeleteWorkspaceAsync(Guid id);
    }
}
