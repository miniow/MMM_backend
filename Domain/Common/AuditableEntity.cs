﻿namespace Domain.Common
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAy { get; set; }
    }
}
