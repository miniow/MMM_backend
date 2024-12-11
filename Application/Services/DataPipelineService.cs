using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class DataPipelineService : IDataPipelineService
    {
        private readonly IDataPipelineRepository _pipelineRepository;
        private readonly IDataFlowRepository _dataFlowRepository;

        public DataPipelineService(IDataPipelineRepository pipelineRepository, IDataFlowRepository dataFlowRepository)
        {
            _pipelineRepository = pipelineRepository;
            _dataFlowRepository = dataFlowRepository;
        }

        public async Task<DataPipeline> CreatePipelineAsync(string name, string userId)
        {
            var pipeline = new DataPipeline
            {
                Id = Guid.NewGuid(),
                Name = name,
                UserId = userId,
                Status = PipelineStatus.Draft,
                CreatedAt = DateTime.UtcNow,
                LastExecutedAt = DateTime.UtcNow,
                DataFlowId = string.Empty
            };

            await _pipelineRepository.AddAsync(pipeline);
            return pipeline;
        }

        public async Task DeletePipelineAsync(Guid id)
        {
            await _pipelineRepository.DeleteAsync(id);

        }

        public async Task<IEnumerable<DataPipeline>> GetAllPipelinesAsync(string userId)
        {
            return await _pipelineRepository.GetByUserIdAsync(userId);
        }

        public async Task<DataPipeline> GetPipelineByIdAsync(Guid id)
        {
            return await _pipelineRepository.GetByIdAsync(id);
        }

        public async Task<DataPipeline> UpdatePipelineDataFlowAsync(Guid pipelineId, string dataFlowJson)
        {
            var pipeline = await _pipelineRepository.GetByIdAsync(pipelineId);
            if (pipeline == null) throw new Exception("Pipeline not found.");

            // Zapis DataFlow do MongoDB
            string dataFlowId = string.Empty;
            if (string.IsNullOrEmpty(pipeline.DataFlowId))
            {
                dataFlowId = await _dataFlowRepository.CreateAsync(dataFlowJson);
                pipeline.DataFlowId = dataFlowId;
            }
            else
            {
                await _dataFlowRepository.UpdateAsync(pipeline.DataFlowId, dataFlowJson);
            }

            pipeline.LastModifiedAy = DateTime.UtcNow;
            await _pipelineRepository.UpdateAsync(pipeline);

            // Możesz załadować DataFlow z MongoDB, jeśli chcesz zwrócić pełny obiekt
            return pipeline;
        }
        //public async Task<string> GetDataFlow(Guid pipelineId)
        //{
        //    var pipeline = await _pipelineRepository.GetByIdAsync(pipelineId);
        //    if (pipeline == null) throw new Exception("Pipeline not found.");
        //}
    }
}
