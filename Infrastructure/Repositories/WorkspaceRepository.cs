using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class WorkspaceRepository : IWorkspaceRepository
    {
        private readonly ApplicationDbContext _context;

        public WorkspaceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Workspace> AddAsync(Workspace workspace)
        {
            await _context.Workspaces.AddAsync(workspace);
            return workspace;
        }

        public async Task DeleteAsync(Guid id)
        {
            var workspace = await GetByIdAsync(id);
            if (workspace != null)
            {
                _context.Workspaces.Remove(workspace);
            }
        }

        public async Task<IEnumerable<Workspace>> GetAllAsync()
        {
            return await _context.Workspaces.ToListAsync();
        }

        public async Task<IEnumerable<Workspace>> GetByUserIdAsync(string userId)
        {
            return await _context.Workspaces
                                 .Where(w => w.UserId == userId)
                                 .ToListAsync();
        }

        public async Task<Workspace> GetByIdAsync(Guid id)
        {
            return await _context.Workspaces.FindAsync(id);
        }

        public async Task<Workspace> UpdateAsync(Workspace workspace)
        {
            _context.Workspaces.Update(workspace);
            return workspace;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
