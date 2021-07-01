using System;

namespace Domain.Common
{
    public abstract class AuditableBaseEntity
    {
        public virtual int Id { get; set; }
        public int CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
