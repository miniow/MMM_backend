using Domain.Common;
using Domain.Interfaces;
using System.Data;

namespace Domain.Entities
{
    public enum PipelineStatus
    {
        Draft,
        Active,
        Inactive,
        Completed,
        Failed
    }



    // Main data pipeline class
    public class DataPipeline : AuditableEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime? LastExecutedAt { get; set; }

        public PipelineStatus Status { get; set; }

        public string UserId { get; set; } = string.Empty;

        // Odnośnik do dokumentu w MongoDB
        public string DataFlowId { get; set; } = string.Empty;
    }

}
