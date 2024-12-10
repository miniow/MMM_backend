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

    // Enumeration for transformation types
    public enum TransformationType
    {
        Mapping,
        Conversion,
        Calculation,
        Concatenation,
        Splitting
    }

    // Enumeration for filter operators
    public enum FilterOperator
    {
        Equals,
        NotEquals,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,
        Contains,
        NotContains
    }

    // Base abstract class for data pipeline entities
    public abstract class DataPipelineBaseEntity
    {
        public Guid Id { get; set; }
    }

    // Represents a data column
    public class DataColumn
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DataType { get; set; } = string.Empty;
        public bool IsNullable { get; set; }
    }

    // Main data pipeline class
    public class DataPipeline : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? LastExecutedAt { get; set; }
        public PipelineStatus Status { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ICollection<DataSource> Sources { get; set; } = new List<DataSource>();
        public ICollection<DataTransformation> Transforms { get; set; } = new List<DataTransformation>();
        public ICollection<Connection> Connections { get; set; } = new List<Connection>();
        public DataDestination Destination { get; set; }

    }

    // Connection class for linking components
    public class Connection 
    {
        public Guid Id { get; set; }
        public Guid SourceId { get; set; }
        public Guid DestinationId { get; set; }
        public DataPipelineBaseEntity Source { get; set; }
        public DataPipelineBaseEntity Destination { get; set; }
    }

    // Abstract base class for data sources
    public class DataSource : DataPipelineBaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ICollection<DataColumn> Columns { get; set; } = new List<DataColumn>();
        public bool IsActive { get; set; } = true;
    }

    // Abstract base class for data transformations
    public class DataTransformation : DataPipelineBaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int ExecutionOrder { get; set; }
        public bool IsActive { get; set; } = true;
    }

    // Column transformation class
    public class ColumnTransformation : DataTransformation
    {
        public DataColumn SourceColumn { get; set; }
        public DataColumn TargetColumn { get; set; }
        public TransformationType TransformationType { get; set; }
        public string? TransformationExpression { get; set; }


    }

    // Filter transformation class
    public class FilterTransformation : DataTransformation
    {
        public string SourceColumn { get; set; } = string.Empty;
        public FilterOperator Operator { get; set; }
        public string? FilterValue { get; set; }
    }

    // Abstract base class for data destinations
    public  class DataDestination : DataPipelineBaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }

}
