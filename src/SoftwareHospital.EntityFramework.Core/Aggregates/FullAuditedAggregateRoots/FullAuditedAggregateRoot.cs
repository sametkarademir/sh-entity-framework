using SoftwareHospital.EntityFramework.Core.Aggregates.AuditedAggregateRoots;

namespace SoftwareHospital.EntityFramework.Core.Aggregates.FullAuditedAggregateRoots;

[Serializable]
public abstract class FullAuditedAggregateRoot : AuditedAggregateRoot, IFullAuditedObject
{
    public virtual bool IsDeleted { get; set; }
    public virtual string? DeleterId { get; set; }
    public virtual DateTime? DeletionTime { get; set; }
    
    public void SetDeleterId(string? deleterId)
    {
        DeleterId = deleterId;
    }
    
    public void SetDeletionTime(DateTime? deletionTime = null)
    {
        DeletionTime = deletionTime ?? DateTime.UtcNow;
    }
}

[Serializable]
public abstract class FullAuditedAggregateRoot<TKey> : AuditedAggregateRoot<TKey>, IFullAuditedObject
{
    public virtual bool IsDeleted { get; set; }

    public virtual string? DeleterId { get; set; }

    public virtual DateTime? DeletionTime { get; set; }
    
    public void SetDeleterId(string? deleterId)
    {
        DeleterId = deleterId;
    }

    public void SetDeletionTime(DateTime? deletionTime = null)
    {
        DeletionTime = deletionTime ?? DateTime.UtcNow;
    }

    protected FullAuditedAggregateRoot()
    {

    }

    protected FullAuditedAggregateRoot(TKey id)
        : base(id)
    {

    }
}
