using Domain.Interfaces;

namespace Infrastructure.Services
{
    public class PipelineExecutionService
    {
        private readonly IDataPipelineRepository _pipelineRepo;

        public PipelineExecutionService(IDataPipelineRepository pipelineRepo)
        {
            _pipelineRepo = pipelineRepo;
        }

        public async Task ExecuteAsync(Guid pipelineId)
        {
            var pipeline = await _pipelineRepo.GetByIdAsync(pipelineId);
            if (pipeline == null)
            {
                throw new Exception("Pipeline not found");
            }

            //var mainTask = _pipelineBuilder.Build(pipeline);
            //if (mainTask == null)
            //{
            //    throw new Exception("No tasks to execute");
            //}

            // Execute pipeline
            // mainTask.Execute();

            // opcjonalnie update statusu pipeline
            pipeline.Status = Domain.Entities.PipelineStatus.Completed;
            pipeline.LastExecutedAt = DateTime.UtcNow;
            await _pipelineRepo.UpdateAsync(pipeline);
        }
    }
}
