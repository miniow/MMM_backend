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
        Task<DataPipelineDto> GetDataPipelineByIdAsync(Guid id);
        Task<IEnumerable<DataPipelineDto>> GetAllDataPipelinesByUserIdAsync(string userId);
        Task<DataPipelineDto> CreateDataPipelineAsync(DataPipelineDto workspaceDto);
        Task<DataPipelineDto> UpdateDataPipelineAsync(DataPipelineDto workspaceDto);
        Task DeleteDataPipelineAsync(Guid id);
    }
}
