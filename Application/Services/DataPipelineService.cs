using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class DataPipelineService : IDataPipelineService
    {
        private readonly IDataPipelineRepository _repository;
        private readonly IMapper _mapper;

        public DataPipelineService(IDataPipelineRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        // Pobranie konkretnego pipeline po Id
        public async Task<DataPipelineDto> GetDataPipelineByIdAsync(Guid id)
        {
            var pipeline = await _repository.GetByIdAsync(id);
            if (pipeline == null)
                return null;

            return _mapper.Map<DataPipelineDto>(pipeline);
        }

        // Utworzenie nowego pipeline
        public async Task<DataPipelineDto> CreateDataPipelineAsync(DataPipelineDto pipelineDto)
        {
            // Mapujemy DTO -> Encja domenowa
            var pipeline = _mapper.Map<DataPipeline>(pipelineDto);
            pipeline.Id = Guid.NewGuid(); // Generujemy nowy Id, jeśli encja nie robi tego sama

            await _repository.AddAsync(pipeline);
            var success = await _repository.SaveChangesAsync();

            if (!success)
                throw new Exception("Failed to create DataPipeline.");

            // Mapujemy z powrotem do DTO (teraz pipeline ma już Id itp.)
            return _mapper.Map<DataPipelineDto>(pipeline);
        }

        // Usunięcie pipeline po Id
        public async Task DeleteDataPipelineAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            var success = await _repository.SaveChangesAsync();

            if (!success)
                throw new Exception("Failed to delete DataPipeline.");
        }

        // Pobranie wszystkich pipeline-ów dla danego użytkownika
        // Zakładamy, że mamy metodę w repozytorium GetByUserIdAsync
        public async Task<IEnumerable<DataPipelineDto>> GetAllDataPipelinesByUserIdAsync(string userId)
        {
            var pipelines = await _repository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<DataPipelineDto>>(pipelines);
        }

        // Aktualizacja pipeline
        public async Task<DataPipelineDto> UpdateDataPipelineAsync(DataPipelineDto pipelineDto)
        {
            var pipeline = await _repository.GetByIdAsync(pipelineDto.Id);
            if (pipeline == null)
                return null;

            // Aktualizujemy właściwości encji
            pipeline.Name = pipelineDto.Name;
            pipeline.Status = pipelineDto.Status;
            pipeline.LastExecutedAt = pipelineDto.LastExecutedAt;

            // Zależnie od potrzeb aktualizujemy także kolekcje Sources, Transforms, Destination itp.
            // Możliwe, że potrzebne będą osobne metody lub logika w repozytorium do aktualizacji powiązań.
            // Przykład prostej aktualizacji Destination (jeśli jest w DTO i wymaga aktualizacji):
            // pipeline.Destination = _mapper.Map<DataDestination>(pipelineDto.Destination);

            await _repository.UpdateAsync(pipeline);
            var success = await _repository.SaveChangesAsync();

            if (!success)
                throw new Exception("Failed to update DataPipeline.");

            return _mapper.Map<DataPipelineDto>(pipeline);
        }
    }
}
