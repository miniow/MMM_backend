using Application.DTOs;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
   public interface IDataPipelineService
    {
        Task<IEnumerable<DataPipeline>> GetAllPipelinesAsync(string userId);
        Task<DataPipeline?> GetPipelineByIdAsync(Guid id);
        Task<DataPipeline> CreatePipelineAsync(string name, string userId);
        Task<DataPipeline> UpdatePipelineDataFlowAsync(Guid pipelineId, string dataFlowJson);
        Task DeletePipelineAsync(Guid id);
    }
}
