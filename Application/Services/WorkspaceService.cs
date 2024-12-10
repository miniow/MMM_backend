using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class WorkspaceService : IWorkspaceService
    {
        private readonly IWorkspaceRepository _workspaceRepository;
        private readonly IMapper _mapper;
        public WorkspaceService(IWorkspaceRepository workspaceRepository, IMapper mapper)
        {
            _workspaceRepository = workspaceRepository;
            _mapper = mapper;
        }
        public async Task<WorkspaceDto> GetWorkspaceByIdAsync(Guid id)
        {
            var workspace = await _workspaceRepository.GetByIdAsync(id);
            if (workspace == null)
                return null;

            return _mapper.Map<WorkspaceDto>(workspace);
        }
        public async Task<WorkspaceDto> CreateWorkspaceAsync(WorkspaceDto workspaceDto)
        {
            var workspace = new Workspace
            {
                Id = Guid.NewGuid(),
                Name = workspaceDto.Name,
                UserId = workspaceDto.UserId
            };

            await _workspaceRepository.AddAsync(workspace);
            var success = await _workspaceRepository.SaveChangesAsync();

            if (!success)
                throw new Exception("Failed to create workspace.");

            workspaceDto.Id = workspace.Id;
            return workspaceDto;
        }
        public async Task DeleteWorkspaceAsync(Guid id)
        {
            await _workspaceRepository.DeleteAsync(id);
            var success = await _workspaceRepository.SaveChangesAsync();

            if (!success)
                throw new Exception("Failed to delete workspace.");
        }
        public async Task<IEnumerable<WorkspaceDto>> GetAllWorkspacesByUserIdAsync(string userId)
        {
            var workspaces = await _workspaceRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<WorkspaceDto>>(workspaces);
        }
        public async Task<WorkspaceDto> UpdateWorkspaceAsync(WorkspaceDto workspaceDto)
        {
            var workspace = await _workspaceRepository.GetByIdAsync(workspaceDto.Id);
            if (workspace == null)
                return null;

            workspace.Name = workspaceDto.Name;
            workspace.UserId = workspaceDto.UserId; // Aktualizacja UserId jeśli potrzebne

            await _workspaceRepository.UpdateAsync(workspace);
            var success = await _workspaceRepository.SaveChangesAsync();

            if (!success)
                throw new Exception("Failed to update workspace.");

            return workspaceDto;
        }
    }
}
