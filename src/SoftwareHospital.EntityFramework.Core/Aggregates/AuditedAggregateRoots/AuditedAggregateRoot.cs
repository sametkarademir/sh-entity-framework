using SoftwareHospital.EntityFramework.Core.Aggregates.CreationAuditedAggregateRoots;

namespace SoftwareHospital.EntityFramework.Core.Aggregates.AuditedAggregateRoots;

[Serializable]
public abstract class AuditedAggregateRoot : CreationAuditedAggregateRoot, IAuditedObject
{
    public virtual DateTime? LastModificationTime { get; protected set; }
    public virtual string? LastModifierId { get; protected set; }

    public void SetLastModificationTime(DateTime? lastModificationTime)
    {
        LastModificationTime = lastModificationTime ?? DateTime.UtcNow;
    }
    
    public void SetLastModifierId(string? lastModifierId)
    {
        LastModifierId = lastModifierId;
    }
}

[Serializable]
public abstract class AuditedAggregateRoot<TKey> : CreationAuditedAggregateRoot<TKey>, IAuditedObject
{
    public virtual DateTime? LastModificationTime { get; protected set; }
    public virtual string? LastModifierId { get; protected set; }
    
    public void SetLastModificationTime(DateTime? lastModificationTime)
    {
        LastModificationTime = lastModificationTime ?? DateTime.UtcNow;
    }
    
    public void SetLastModifierId(string? lastModifierId)
    {
        LastModifierId = lastModifierId;
    }

    protected AuditedAggregateRoot()
    {

    }

    protected AuditedAggregateRoot(TKey id) : base(id)
    {

    }
}