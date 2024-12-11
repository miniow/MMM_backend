using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{

    public class DataPipelineDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? LastExecutedAt { get; set; }
        public PipelineStatus Status { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string DataFlowId { get; set; } = string.Empty;
    }
    public class UpdateDataPipelineDto
    {
        public string Name { get; set; } = string.Empty;
        public PipelineStatus Status { get; set; }
        public string DataFlowId { get; set; } = string.Empty;
    }
    public class UpdateDataFlowDto
    {
        public string DataFlowJson { get; set; } = string.Empty;
    }
}
