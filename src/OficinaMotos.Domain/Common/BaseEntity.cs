using System;

namespace OficinaMotos.Domain.Common
{
    public abstract class BaseEntity
    {
        // Use long to mirror the bigint primary keys defined in the database script
        public long Id { get; protected set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        protected BaseEntity()
        {
            CreatedAt = DateTime.UtcNow;
            IsDeleted = false;
        }

        // Marks the entity as deleted without removing the row
        public void Delete()
        {
            if (IsDeleted) return;

            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            UpdatedAt = DeletedAt;
        }

        // Updates the timestamp when a change happens
        public void SetUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
