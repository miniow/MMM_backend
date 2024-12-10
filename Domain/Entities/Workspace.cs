using Domain.Common;

namespace Domain.Entities
{
    public class Workspace : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
        public bool IsFavorite { get; set; }

        public DataPipeline? DataPipeline { get; set; }
    }
}
