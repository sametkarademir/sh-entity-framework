using SoftwareHospital.EntityFramework.Aggregates.AggregateRoots;

namespace SoftwareHospital.EntityFramework.Aggregates.CreationAuditedAggregateRoots;

[Serializable]
public abstract class CreationAuditedAggregateRoot<TKey> : AggregateRoot<TKey>, ICreationAuditedObject
{
    public virtual DateTime CreationTime { get; protected set; }
    public virtual string? CreatorId { get; protected set; }
    
    public void SetCreationTime(DateTime? creationTime)
    {
        CreationTime = creationTime ?? DateTime.UtcNow;
    }
    
    public void SetCreatorId(string? creatorId)
    {
        CreatorId = creatorId;
    }

    protected CreationAuditedAggregateRoot()
    {

    }

    protected CreationAuditedAggregateRoot(TKey id)
        : base(id)
    {

    }
}

[Serializable]
public abstract class CreationAuditedAggregateRoot : AggregateRoot, ICreationAuditedObject
{
    public virtual DateTime CreationTime { get; protected set; }
    public virtual string? CreatorId { get; protected set; }

    public void SetCreationTime(DateTime? creationTime)
    {
        CreationTime = creationTime ?? DateTime.UtcNow;
    }

    public void SetCreatorId(string? creatorId)
    {
        CreatorId = creatorId;
    }
}