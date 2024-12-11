using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DataPipelineRepository : IDataPipelineRepository
    {
        private readonly ApplicationDbContext _context;

        public DataPipelineRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DataPipeline> AddAsync(DataPipeline dataPipeline)
        {
            await _context.DataPipelines.AddAsync(dataPipeline);
            await _context.SaveChangesAsync();
            return dataPipeline;
        }

        public async Task DeleteAsync(Guid id)
        {
            var dataPipeline = await GetByIdAsync(id);
            if (dataPipeline != null)
            {
                _context.DataPipelines.Remove(dataPipeline);
            }
        }

        public async Task<DataPipeline> GetByIdAsync(Guid id)
        {
            return await _context.DataPipelines
              .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<DataPipeline>> GetByUserIdAsync(string userId)
        {
            return await _context.DataPipelines
              .Where(d => d.UserId == userId)
              .ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<DataPipeline> UpdateAsync(DataPipeline dataPipeline)
        {
            _context.DataPipelines.Update(dataPipeline);
            return dataPipeline;
        }
    }
}
